namespace Uceme.Library.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Uceme.Model.DataContracts;
    using Uceme.Model.Models;

    public interface IAppointmentService
    {
        Task<bool> AddAppointmentAsync(AppointmentRequest appointmentRequest);

        IEnumerable<int> GetDays(int hospitalId);

        IEnumerable<string> GetHours(AppointmentHoursRequest appointmentHoursRequest);

        IEnumerable<Appointment> GetAppointments();

        IEnumerable<Appointment> GetCloseAppointments();

        Appointment GetAppointment(int appointmentId);

        bool DeleteAppointment(int appointmentId);

        Appointment UpdateAppointment(Cita appointment);

        bool UpdatePastAppointmentsData();
    }
}
