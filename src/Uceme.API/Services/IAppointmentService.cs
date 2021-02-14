namespace Uceme.API.Services
{
    using System.Collections.Generic;
    using Uceme.Model.DataContracts;

    public interface IAppointmentService
    {
        bool AddAppointment(AppointmentRequest appointmentRequest);

        IEnumerable<int> GetDays(int hospitalId);

        IEnumerable<string> GetHours(AppointmentHoursRequest appointmentHoursRequest);
    }
}