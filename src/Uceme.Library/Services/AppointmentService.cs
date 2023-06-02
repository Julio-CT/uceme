namespace Uceme.Library.Services;

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Uceme.Model.Data;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;

public class AppointmentService : IAppointmentService
{
    private readonly ILogger<AppointmentService> logger;

    private readonly IEmailService emailService;

    private readonly ApplicationDbContext context;

    public AppointmentService(
        ILogger<AppointmentService> logger,
        IApplicationDbContext context,
        IEmailService emailService)
    {
        this.logger = logger;
        this.context = (ApplicationDbContext)context;
        this.emailService = emailService;
    }

    public IEnumerable<Appointment> GetAppointments()
    {
        try
        {
            IOrderedQueryable<Cita> existingAppointments = this.context.Cita.OrderByDescending(a => a.dia).ThenByDescending(a => a.hora);

            return this.MapCitasToAppointments(existingAppointments);
        }
        catch (Exception e)
        {
            this.logger.LogError($"Error retrieving appointments {e.Message}");
            throw new DataException("Error retrieving appointments", e);
        }
    }

    public IEnumerable<Appointment> GetCloseAppointments()
    {
        try
        {
            string todaysYear = DateTime.Now.Year.ToString(CultureInfo.CurrentCulture);
            string todaysMonth = DateTime.Now.Month.ToString(CultureInfo.CurrentCulture);
            todaysMonth = todaysMonth.Length > 1 ? todaysMonth : "0" + todaysMonth;
            string todaysDay = DateTime.Now.Day.ToString(CultureInfo.CurrentCulture);
            todaysDay = todaysDay.Length > 1 ? todaysDay : "0" + todaysDay;
            uint todaysDate = Convert.ToUInt32(todaysYear + todaysMonth + todaysDay, CultureInfo.CurrentCulture);
            string tomorrowsYear = DateTime.Now.AddDays(2).Year.ToString(CultureInfo.CurrentCulture);
            string tomorrowsMonth = DateTime.Now.AddDays(2).Month.ToString(CultureInfo.CurrentCulture);
            tomorrowsMonth = tomorrowsMonth.Length > 1 ? tomorrowsMonth : "0" + tomorrowsMonth;
            string tomorrowsDay = DateTime.Now.AddDays(2).Day.ToString(CultureInfo.CurrentCulture);
            tomorrowsDay = tomorrowsDay.Length > 1 ? tomorrowsDay : "0" + tomorrowsDay;
            uint tomorrowsDate = Convert.ToUInt32(tomorrowsYear + tomorrowsMonth + tomorrowsDay, CultureInfo.CurrentCulture);
            IOrderedQueryable<Cita> existingAppointments = this.context.Cita.Where(a => a.dia <= tomorrowsDate && a.dia >= todaysDate).OrderByDescending(a => a.dia).ThenByDescending(a => a.hora);

            return this.MapCitasToAppointments(existingAppointments);
        }
        catch (Exception e)
        {
            this.logger.LogError($"Error retrieving appointments {e.Message}");
            throw new DataException("Error retrieving appointments", e);
        }
    }

    public IEnumerable<string> GetHours(AppointmentHoursRequest appointmentHoursRequest)
    {
        if (appointmentHoursRequest == null)
        {
            throw new ArgumentNullException(nameof(appointmentHoursRequest));
        }

        try
        {
            int americanDate = appointmentHoursRequest.Day
                + ((appointmentHoursRequest.Month + 1) * 100)
                + (appointmentHoursRequest.Year * 10000);
            int hospitalId = Convert.ToInt32(appointmentHoursRequest.HospitalId, CultureInfo.CurrentCulture);
            int weekday = appointmentHoursRequest.WeekDay;

            List<string> result = new List<string>();

            List<Turno> shifts = this.context.Turno.Where(o => o.idHospital == hospitalId && o.dia == weekday).ToList();
            foreach (Turno? shift in shifts)
            {
                List<Cita> existingAppointments = this.context.Cita.Where(o => o.dia == americanDate && o.idTurno == shift.idTurno).ToList();
                decimal increment = 1 / Convert.ToDecimal(shift.porhora);

                for (int i = 0; i < shift.paralelas; i++)
                {
                    for (decimal j = shift.inicio; j <= shift.fin; j += increment)
                    {
                        result.Add(UCEME.Utilities.DateTimeUtils.TimeToString(j));
                    }
                }

                foreach (Cita? appointment in existingAppointments)
                {
                    string appointmentTime = UCEME.Utilities.DateTimeUtils.TimeToString(appointment.hora);
                    result.Remove(appointmentTime);
                }
            }

            result = result.OrderBy(o => o).Distinct().ToList();

            return result;
        }
        catch (Exception e)
        {
            this.logger.LogError($"Error retrieving hours {e.Message}");
            throw new DataException("Error retrieving hours", e);
        }
    }

    public IEnumerable<int> GetDays(int hospitalId)
    {
        try
        {
            List<int> data = this.context.Turno.Where(o => o.idHospital == hospitalId).Select(o => o.dia).ToList();
            return data;
        }
        catch (Exception e)
        {
            this.logger.LogError($"Error retrieving days {e.Message}");
            throw new DataException("Error retrieving days", e);
        }
    }

    public async Task<bool> AddAppointmentAsync(AppointmentRequest appointmentRequest)
    {
        if (appointmentRequest == null || appointmentRequest.Hour == null)
        {
            throw new ArgumentNullException(nameof(appointmentRequest));
        }

        try
        {
            Cita cita = new Cita
            {
                dia = appointmentRequest.Day
                + (appointmentRequest.Month * 100)
                + (appointmentRequest.Year * 10000),
                hora = UCEME.Utilities.DateTimeUtils.TimeToDecimal(appointmentRequest.Hour),
                nombre = appointmentRequest.Name,
                telefono = appointmentRequest.Phone,
            };

            int weekday = Convert.ToInt32(appointmentRequest.WeekDay, CultureInfo.CurrentCulture);

            Turno turno = this.context.Turno.First(o => o.idHospital == appointmentRequest.HospitalId && o.dia == weekday);
            cita.idTurno = turno.idTurno;
            if (!string.IsNullOrEmpty(appointmentRequest.Email))
            {
                cita.email = appointmentRequest.Email;
            }

            this.context.Cita.Add(cita);
            this.context.SaveChanges();

            return await this.SendAppointmentEmailAsync(appointmentRequest, cita).ConfigureAwait(false);
        }
        catch (OperationCanceledException opex)
        {
            this.logger.LogError($"Appointment added, error sending message {opex.Message}");
            return false;
        }
        catch (Exception e)
        {
            this.logger.LogError($"Error adding appointment {e.Message}");
            throw new DataException("Error adding appointment", e);
        }
    }

    public bool UpdatePastAppointmentsData()
    {
        try
        {
            string todaysYear = DateTime.Now.AddDays(-7).Year.ToString(CultureInfo.CurrentCulture);
            string todaysMonth = DateTime.Now.AddDays(-7).Month.ToString(CultureInfo.CurrentCulture);
            todaysMonth = todaysMonth.Length > 1 ? todaysMonth : "0" + todaysMonth;
            string todaysDay = DateTime.Now.AddDays(-7).Day.ToString(CultureInfo.CurrentCulture);
            uint todaysDate = Convert.ToUInt32(todaysYear + todaysMonth + todaysDay, CultureInfo.CurrentCulture);

            IQueryable<Cita> existingAppointments = this.context.Cita.Where(a => a.dia < todaysDate);

            foreach (Cita? existingAppointment in existingAppointments.ToList())
            {
                existingAppointment.email = "eliminado";
                existingAppointment.telefono = "eliminado";

                this.context.Cita.Update(existingAppointment);
            }

            this.context.SaveChanges();

            return true;
        }
        catch (Exception e)
        {
            this.logger.LogError($"Error retrieving appointments {e.Message}");
            throw new DataException("Error retrieving appointments", e);
        }
    }

    Appointment IAppointmentService.GetAppointment(int appointmentId)
    {
        try
        {
            Cita cita = this.context.Cita.First(cita => cita.idCita == appointmentId);

            return this.MapCitaToAppointment(cita);
        }
        catch (Exception e)
        {
            this.logger.LogError($"Error finding appointment {e.Message}");
            throw new DataException("Error finding appointment", e);
        }
    }

    bool IAppointmentService.DeleteAppointment(int appointmentId)
    {
        try
        {
            Cita appointment = this.context.Cita.First(cita => cita.idCita == appointmentId);
            _ = this.context.Cita.Remove(appointment);
            this.context.SaveChanges();

            return true;
        }
        catch (Exception e)
        {
            this.logger.LogError($"Error deleting appointment {e.Message}");
            throw new DataException("Error deleting appointment", e);
        }
    }

    Appointment IAppointmentService.UpdateAppointment(Cita appointment)
    {
        try
        {
            Cita existingAppointment = this.context.Cita.First(cita => cita.idCita == appointment.idCita);

            existingAppointment.dia = appointment.dia;
            existingAppointment.email = appointment.email;
            existingAppointment.hora = appointment.hora;
            existingAppointment.idTurno = appointment.idTurno;
            existingAppointment.nombre = appointment.nombre;
            existingAppointment.telefono = appointment.telefono;

            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Cita> result = this.context.Cita.Update(existingAppointment);
            this.context.SaveChanges();

            return this.MapCitaToAppointment(result.Entity);
        }
        catch (Exception e)
        {
            this.logger.LogError($"Error updating appointment {e.Message}");
            throw new DataException("Error updating appointment", e);
        }
    }

    private List<Appointment> MapCitasToAppointments(IOrderedQueryable<Cita> existingAppointments)
    {
        List<Appointment> response = new List<Appointment>();
        foreach (Cita existingAppointment in existingAppointments)
        {
            response.Add(this.MapCitaToAppointment(existingAppointment));
        }

        return response;
    }

    private Appointment MapCitaToAppointment(Cita existingAppointment)
    {
        Turno turno = this.context.Turno.First(x => x.idTurno == existingAppointment.idTurno);
        return new Appointment()
        {
            dia = existingAppointment.dia,
            hora = existingAppointment.hora,
            email = existingAppointment.email,
            idCita = existingAppointment.idCita,
            idTurno = existingAppointment.idTurno,
            nombre = existingAppointment.nombre,
            telefono = existingAppointment.telefono,
            speciality = this.context.DatosProfesionales.First(x => x.idDatosPro == turno.idHospital).nombre,
        };
    }

    private async Task<bool> SendAppointmentEmailAsync(AppointmentRequest appointmentRequest, Cita cita)
    {
        StringBuilder emailMessage = new StringBuilder();

        emailMessage.Append("Notifiación: ");
        emailMessage.Append("<br />");
        emailMessage.Append("<br />");
        emailMessage.Append("Hay una nueva cita de UCEME:");
        emailMessage.Append("<br />");
        emailMessage.Append("<br />");
        emailMessage.Append("El paciente " + cita.nombre + " tiene una cita el dia "
            + appointmentRequest.Day + "/" + appointmentRequest.Month + "/" + appointmentRequest.Year
            + " a las " + appointmentRequest.Hour + ".");
        emailMessage.Append("<br />");
        emailMessage.Append("Su teléfono es : " + cita.telefono + ".");
        emailMessage.Append("<br />");
        if (!string.IsNullOrEmpty(cita.email))
        {
            emailMessage.Append("y su email es: " + cita.email);
        }
        else
        {
            emailMessage.Append("y no dejó email de contacto.");
        }

        if (!string.IsNullOrEmpty(appointmentRequest.ExtraInfo))
        {
            emailMessage.Append("<br />");
            emailMessage.Append("Adjuntó las siguientes observaciones : " + appointmentRequest.ExtraInfo);
        }

        emailMessage.Append("<br />");
        emailMessage.Append("Por favor, no responda a este email, para cualquier cambio o duda use el formulario de contacto de la web.");
        emailMessage.Append("<br />");

        return await this.emailService.SendEmailToManagementAsync(cita.email, "Nueva cita en Uceme", emailMessage.ToString()).ConfigureAwait(false);
    }
}
