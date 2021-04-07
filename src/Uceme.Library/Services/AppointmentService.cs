﻿namespace Uceme.Library.Services
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

        private readonly ApplicationDbContext context;

        public AppointmentService(
            ILogger<HospitalService> logger,
            ApplicationDbContext context,
            IEmailService emailService)
        {
            this.logger = logger;
            this.context = context;
            this.emailService = emailService;
        }

        public IEnumerable<Cita> GetAppointments()
        {
            try
            {
                var existingAppointments = this.context.Cita.OrderByDescending(a => a.dia).ThenByDescending(a => a.hora);

                return existingAppointments;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error retrieving appointments {e.Message}");
                throw new DataException("Error retrieving appointments", e);
            }
        }

        public IEnumerable<Cita> GetCloseAppointments()
        {
            try
            {
                var todaysYear = DateTime.Now.Year.ToString(CultureInfo.CurrentCulture);
                var todaysMonth = DateTime.Now.Month.ToString(CultureInfo.CurrentCulture);
                todaysMonth = todaysMonth.Length > 1 ? todaysMonth : "0" + todaysMonth;
                var todaysDay = DateTime.Now.Day.ToString(CultureInfo.CurrentCulture);
                var todaysDate = Convert.ToUInt32(todaysYear + todaysMonth + todaysDay, CultureInfo.CurrentCulture);
                var tomorrowsYear = DateTime.Now.AddDays(2).Year.ToString(CultureInfo.CurrentCulture);
                var tomorrowsMonth = DateTime.Now.AddDays(2).Month.ToString(CultureInfo.CurrentCulture);
                tomorrowsMonth = tomorrowsMonth.Length > 1 ? tomorrowsMonth : "0" + tomorrowsMonth;
                var tomorrowsDay = DateTime.Now.AddDays(2).Day.ToString(CultureInfo.CurrentCulture);
                var tomorrowsDate = Convert.ToUInt32(tomorrowsYear + tomorrowsMonth + tomorrowsDay, CultureInfo.CurrentCulture);
                var existingAppointments = this.context.Cita.Where(a => a.dia <= tomorrowsDate && a.dia >= todaysDate).OrderByDescending(a => a.dia).ThenByDescending(a => a.hora);

                return existingAppointments;
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
                var americanDate = appointmentHoursRequest.Day
                    + ((appointmentHoursRequest.Month + 1) * 100)
                    + (appointmentHoursRequest.Year * 10000);
                var hospitalId = Convert.ToInt32(appointmentHoursRequest.HospitalId, CultureInfo.CurrentCulture);
                var weekday = appointmentHoursRequest.WeekDay;

                var result = new List<string>();

                var shifts = this.context.Turno.Where(o => o.idHospital == hospitalId && o.dia == weekday).ToList();
                foreach (var shift in shifts)
                {
                    var existingAppointments = this.context.Cita.Where(o => o.dia == americanDate && o.idTurno == shift.idTurno).ToList();
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
                this.logger.LogError($"Error retrieving hours {e.Message}");
                throw new DataException("Error retrieving hours", e);
            }
        }

        public IEnumerable<int> GetDays(int hospitalId)
        {
            try
            {
                var data = this.context.Turno.Where(o => o.idHospital == hospitalId).Select(o => o.dia).ToList();
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
            if (appointmentRequest == null)
            {
                throw new ArgumentNullException(nameof(appointmentRequest));
            }

            try
            {
                var cita = new Cita
                {
                    dia = appointmentRequest.Day
                    + (appointmentRequest.Month * 100)
                    + (appointmentRequest.Year * 10000),
                    hora = UCEME.Utilities.DateTimeUtils.TimeToDecimal(appointmentRequest.Hour),
                    nombre = appointmentRequest.Name,
                    telefono = appointmentRequest.Phone,
                };

                var weekday = Convert.ToInt32(appointmentRequest.WeekDay, CultureInfo.CurrentCulture);

                var turno = this.context.Turno.FirstOrDefault(o => o.idHospital == appointmentRequest.HospitalId && o.dia == weekday);
                cita.idTurno = turno.idTurno;
                if (!string.IsNullOrEmpty(appointmentRequest.Email))
                {
                    cita.email = appointmentRequest.Email;
                }

                this.context.Cita.Add(cita);
                this.context.SaveChanges();

                var result = await this.SendAppointmentEmailAsync(appointmentRequest, cita).ConfigureAwait(false);

                return result;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error adding appointment {e.Message}");
                throw new DataException("Error adding appointment", e);
            }
        }

        Cita IAppointmentService.GetAppointment(int appointmentId)
        {
            try
            {
                var cita = this.context.Cita.FirstOrDefault(cita => cita.idCita == appointmentId);

                return cita;
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
                var cita = this.context.Cita.FirstOrDefault(cita => cita.idCita == appointmentId);
                var result = this.context.Cita.Remove(cita);
                this.context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error deleting appointment {e.Message}");
                throw new DataException("Error deleting appointment", e);
            }
        }

        Cita IAppointmentService.UpdateAppointment(Cita appointment)
        {
            try
            {
                var cita = this.context.Cita.FirstOrDefault(cita => cita.idCita == appointment.idCita);

                cita.dia = appointment.dia;
                cita.email = appointment.email;
                cita.hora = appointment.hora;
                cita.idTurno = cita.idTurno;
                cita.nombre = cita.nombre;
                cita.telefono = cita.telefono;

                var result = this.context.Cita.Update(cita);
                this.context.SaveChanges();

                return result.Entity;
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error updating appointment {e.Message}");
                throw new DataException("Error updating appointment", e);
            }
        }

        private async Task<bool> SendAppointmentEmailAsync(AppointmentRequest appointmentRequest, Cita cita)
        {
            var emailMessage = new StringBuilder();

            emailMessage.Append("<br />");
            emailMessage.Append("Hay una nueva cita de UCEME: ");
            emailMessage.Append("<br />");
            emailMessage.Append("El paciente " + cita.nombre + " tiene una cita el dia " + appointmentRequest.Day +
                    "/" + (appointmentRequest.Month + 1) +
                    "/" + appointmentRequest.Year + " a las " + appointmentRequest.Hour);
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

            return await this.emailService.SendEmailToManagementAsync(cita.email, "Nueva cita en Uceme", emailMessage.ToString()).ConfigureAwait(false);
        }
    }
}
