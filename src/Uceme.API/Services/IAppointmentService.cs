namespace Uceme.API.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Uceme.Model.DataContracts;

    public interface IAppointmentService
    {
        Task<bool> AddAppointmentAsync(AppointmentRequest appointmentRequest);

        IEnumerable<int> GetDays(int hospitalId);

        IEnumerable<string> GetHours(AppointmentHoursRequest appointmentHoursRequest);
    }
}