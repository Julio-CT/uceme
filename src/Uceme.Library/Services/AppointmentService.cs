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
    public const int DataExpiryDays = -7;

    private readonly ILogger<AppointmentService> logger;

    private readonly IEmailService emailService;

    private readonly ApplicationDbContext context;

    public AppointmentService(
        ILogger<AppointmentService> logger,
        IApplicationDbContext context,
        IEmailService emailService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.context = (ApplicationDbContext)context ?? throw new ArgumentNullException(nameof(context));
        this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    public IEnumerable<CalendarEvent>? GetAppointmentsEvents()
    {
        try
        {
            uint pastDate = GetUintDate(-30);

            IOrderedQueryable<Cita>? existingAppointments = this.context.Cita.Where(a => a.dia > pastDate)?.OrderByDescending(a => a.dia).ThenByDescending(a => a.hora);

            return existingAppointments != null && existingAppointments.Any() ? this.MapCitasToAppointmentsEvents(existingAppointments) : null;
        }
        catch (Exception e)
        {
            this.logger.LogError("Error retrieving appointments {EMessage}", e.Message);
            throw new DataException("Error retrieving appointments", e);
        }
    }

    public IEnumerable<Appointment>? GetAppointments()
    {
        try
        {
            uint pastDate = GetUintDate(-30);

            IOrderedQueryable<Cita>? existingAppointments = this.context.Cita.Where(a => a.dia > pastDate)?.OrderByDescending(a => a.dia).ThenByDescending(a => a.hora);

            return existingAppointments != null && existingAppointments.Any() ? this.MapCitasToAppointments(existingAppointments) : null;
        }
        catch (Exception e)
        {
            this.logger.LogError("Error retrieving appointments {EMessage}", e.Message);
            throw new DataException("Error retrieving appointments", e);
        }
    }

    public IEnumerable<Appointment> GetCloseAppointments()
    {
        try
        {
            uint todaysDate = GetUintDate(0);

            uint tomorrowsDate = GetUintDate(2);

            IOrderedQueryable<Cita> existingAppointments = this.context.Cita.Where(a => a.dia <= tomorrowsDate && a.dia >= todaysDate).OrderByDescending(a => a.dia).ThenByDescending(a => a.hora);

            return this.MapCitasToAppointments(existingAppointments);
        }
        catch (Exception e)
        {
            this.logger.LogError("Error retrieving appointments {EMessage}", e.Message);
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
                decimal increment = 1 / Convert.ToDecimal(shift.porhora);

                for (int i = 0; i < shift.paralelas; i++)
                {
                    for (decimal j = shift.inicio; j <= shift.fin; j += increment)
                    {
                        result.Add(UCEME.Utilities.DateTimeUtils.TimeToString(j));
                    }
                }

                if (result.Any())
                {
                    List<Cita> existingAppointments = this.context.Cita.Where(o => o.dia == americanDate && o.idTurno == shift.idTurno).ToList();
                    foreach (Cita? appointment in existingAppointments)
                    {
                        string appointmentTime = UCEME.Utilities.DateTimeUtils.TimeToString(appointment.hora);
                        result.Remove(appointmentTime);
                    }
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

            Turno turno = this.context.Turno.First(o => o.idHospital == appointmentRequest.HospitalId && o.dia == appointmentRequest.WeekDay);
            cita.idTurno = turno.idTurno;
            if (!string.IsNullOrEmpty(appointmentRequest.Email))
            {
                cita.email = appointmentRequest.Email;
            }

            this.context.Cita.Add(cita);
            await this.context.SaveChangesAsync().ConfigureAwait(false);

            return await this.SendAppointmentEmailAsync(appointmentRequest, cita).ConfigureAwait(false);
        }
        catch (OperationCanceledException opex)
        {
            this.logger.LogError("Appointment added, error sending message {OpexMessage}", opex.Message);
            return false;
        }
        catch (Exception e)
        {
            this.logger.LogError("Error adding appointment {EMessage}", e.Message);
            throw new DataException("Error adding appointment", e);
        }
    }

    public bool UpdatePastAppointmentsData()
    {
        try
        {
            uint pastDate = GetUintDate(DataExpiryDays);

            IQueryable<Cita> existingAppointments = this.context.Cita.Where(a => a.dia < pastDate);

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
            this.logger.LogError("Error retrieving appointments {EMessage}", e.Message);
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
            this.logger.LogError("Error finding appointment {EMessage}", e.Message);
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
            this.logger.LogError("Error deleting appointment {EMessage}", e.Message);
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
            this.logger.LogError("Error updating appointment {EMessage}", e.Message);
            throw new DataException("Error updating appointment", e);
        }
    }

    private static uint GetUintDate(int delta)
    {
        string tomorrowsYear = DateTime.Now.AddDays(delta).Year.ToString(CultureInfo.CurrentCulture);
        string tomorrowsMonth = DateTime.Now.AddDays(delta).Month.ToString("00", CultureInfo.CurrentCulture);
        string tomorrowsDay = DateTime.Now.AddDays(delta).Day.ToString("00", CultureInfo.CurrentCulture);
        uint tomorrowsDate = Convert.ToUInt32(tomorrowsYear + tomorrowsMonth + tomorrowsDay, CultureInfo.CurrentCulture);
        return tomorrowsDate;
    }

    private static string ParseEventDate(int dia, decimal hora)
    {
        string dateString = dia.ToString(CultureInfo.InvariantCulture);
        string year = dateString.Substring(0, 4);
        int month = Convert.ToInt32(dateString.Substring(4, 2), CultureInfo.InvariantCulture) - 1;
        string day = dateString.Substring(6, 2);
        int hours = (int)hora;
        int minutes = (int)(60 * (hora - hours));
        return year + "-" + month + "-" + day + "-" + hours + "-" + minutes;
    }

    private List<CalendarEvent> MapCitasToAppointmentsEvents(IOrderedQueryable<Cita> existingAppointments)
    {
        List<CalendarEvent> response = new List<CalendarEvent>();
        foreach (Cita existingAppointment in existingAppointments)
        {
            response.Add(this.MapCitaToAppointmentEvents(existingAppointment));
        }

        return response;
    }

    private CalendarEvent MapCitaToAppointmentEvents(Cita existingAppointment)
    {
        Turno turno = this.context.Turno.First(x => x.idTurno == existingAppointment.idTurno);
        return new CalendarEvent()
        {
            id = existingAppointment.idCita,
            title = this.context.DatosProfesionales.First(x => x.idDatosPro == turno.idHospital).nombre + ": " + existingAppointment.nombre + ".",
            description = "Telf: " + existingAppointment.telefono + ", Email: " + existingAppointment.email,
            start = ParseEventDate(existingAppointment.dia, existingAppointment.hora),
            end = ParseEventDate(existingAppointment.dia, existingAppointment.hora + (turno.porhora != 1 ? (1M / turno.porhora) : 0M)),
        };
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
