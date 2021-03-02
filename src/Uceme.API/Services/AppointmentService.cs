namespace Uceme.API.Services
{
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
        private readonly ILogger<HospitalService> logger;

        private readonly IEmailService emailService;

        private readonly ApplicationDbContext dbContext;

        public AppointmentService(
            ILogger<HospitalService> logger,
            ApplicationDbContext context,
            IEmailService emailService)
        {
            this.logger = logger;
            this.dbContext = context;
            this.emailService = emailService;
        }

        public IEnumerable<string> GetHours(AppointmentHoursRequest appointmentHoursRequest)
        {
            if (appointmentHoursRequest == null)
            {
                throw new ArgumentNullException(nameof(appointmentHoursRequest));
            }

            try
            {
                var americanDate = appointmentHoursRequest.Day
                    + (appointmentHoursRequest.Month + 1) * 100
                    + appointmentHoursRequest.Year * 10000;
                var hospitalId = Convert.ToInt32(appointmentHoursRequest.HospitalId, CultureInfo.CurrentCulture);
                var weekday = appointmentHoursRequest.WeekDay;

                var result = new List<string>();

                var shifts = this.dbContext.Turno.Where(o => o.idHospital == hospitalId && o.dia == weekday).ToList();
                foreach (var shift in shifts)
                {
                    var existingAppointments = this.dbContext.Cita.Where(o => o.dia == americanDate && o.idTurno == shift.idTurno).ToList();
                    var increment = 1 / Convert.ToDecimal(shift.porhora);

                    for (var i = 0; i < shift.paralelas; i++)
                    {
                        for (var j = shift.inicio; j <= shift.fin; j += increment)
                        {
                            result.Add(UCEME.Utilities.DateTimeUtils.TimeToString(j));
                        }
                    }

                    foreach (var appointment in existingAppointments)
                    {
                        var appointmentTime = UCEME.Utilities.DateTimeUtils.TimeToString(appointment.hora);
                        result.Remove(appointmentTime);
                    }
                }

                result = result.OrderBy(o => o).Distinct().ToList();

                return result;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error retrieving Hospitals {e.Message}");
                throw new DataException("Error retrieving Hospitals", e);
            }
        }

        public IEnumerable<int> GetDays(int hospitalId)
        {
            try
            {
                var data = this.dbContext.Turno.Where(o => o.idHospital == hospitalId).Select(o => o.dia).ToList();
                return data;

            }
            catch (Exception e)
            {
                this.logger.LogError($"Error retrieving Hospital {e.Message}");
                throw new DataException("Error retrieving Hospital", e);
            }
        }

        public async Task<bool> AddAppointmentAsync(AppointmentRequest appointmentRequest)
        {
            if (appointmentRequest == null)
            {
                throw new ArgumentNullException(nameof(appointmentRequest));
            }

            try
            {
                var cita = new Cita
                {
                    dia = appointmentRequest.Day
                    + (appointmentRequest.Month + 1) * 100
                    + appointmentRequest.Year * 10000,
                    hora = UCEME.Utilities.DateTimeUtils.TimeToDecimal(appointmentRequest.Hour),
                    nombre = appointmentRequest.Name,
                    telefono = appointmentRequest.Phone,
                };

                var weekday = Convert.ToInt32(appointmentRequest.WeekDay, CultureInfo.CurrentCulture);
                var hospitalId = Convert.ToInt32(appointmentRequest.HospitalId, CultureInfo.CurrentCulture);

                var turno = this.dbContext.Turno.FirstOrDefault(o => o.idHospital == hospitalId && o.dia == weekday);
                cita.idTurno = turno.idTurno;
                if (!string.IsNullOrEmpty(appointmentRequest.Email))
                {
                    cita.email = appointmentRequest.Email;
                }

                var result = await SendAppointmentEmailAsync(appointmentRequest, cita).ConfigureAwait(false);

                this.dbContext.Cita.Add(cita);
                this.dbContext.SaveChanges();

                return result;

            }
            catch (Exception e)
            {
                this.logger.LogError($"Error retrieving Hospital {e.Message}");
                throw new DataException("Error retrieving Hospital", e);
            }
        }

        private async Task<bool> SendAppointmentEmailAsync(AppointmentRequest appointmentRequest, Cita cita)
        {
            var emailMessage = new StringBuilder();

            emailMessage.Append("<br />");
            emailMessage.Append("Hay una nueva cita de UCEME: ");
            emailMessage.Append("<br />");
            emailMessage.Append("El paciente " + cita.nombre + " tiene una cita el dia " + cita.dia + " a las " + cita.hora);
            emailMessage.Append("<br />");
            emailMessage.Append("Su telefono es : " + cita.telefono);
            emailMessage.Append("<br />");
            if (!string.IsNullOrEmpty(cita.email))
            {
                emailMessage.Append("y su email es: " + cita.email);
            }
            else
            {
                emailMessage.Append("y no dejo email de contacto");
            }

            if (!string.IsNullOrEmpty(appointmentRequest.ExtraInfo))
            {
                emailMessage.Append("<br />");
                emailMessage.Append("Adjuntó las siguientes observaciones : " + appointmentRequest.ExtraInfo);
            }

            return await emailService.SendEmailToManagementAsync(cita.email, "Nueva cita en Uceme", emailMessage.ToString()).ConfigureAwait(false);
        }
    }
}
