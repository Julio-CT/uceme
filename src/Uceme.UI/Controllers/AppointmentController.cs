namespace Uceme.UI.Controllers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Uceme.Library.Services;
    using Uceme.Model.Models;

    [Authorize]
    [Route("clientapi/[controller]")]
    [ApiController]
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> logger;

        private readonly IAppointmentService appointmentService;

        public AppointmentController(
            ILogger<AppointmentController> logger,
            IAppointmentService appointmentService)
        {
            this.logger = logger;
            this.appointmentService = appointmentService;
        }

        [HttpGet("appointmentlist")]
        public ActionResult<IEnumerable<Cita>> AppointmentList()
        {
            IEnumerable<Cita> result = null;
            try
            {
                result = this.appointmentService.GetAppointments();
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result.ToList();
        }

        [HttpGet("closeappointmentlist")]
        public ActionResult<IEnumerable<Cita>> CloseAppointmentList()
        {
            IEnumerable<Cita> result = null;
            try
            {
                result = this.appointmentService.GetCloseAppointments();
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result.ToList();
        }

        [HttpGet("getappointment")]
        public ActionResult<Cita> GetAppointment(int appointmentId)
        {
            Cita result = null;
            try
            {
                result = this.appointmentService.GetAppointment(appointmentId);
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result;
        }

        [HttpGet("deleteappointment")]
        public ActionResult<bool> DeleteAppointment(int appointmentId)
        {
            bool result = false;
            try
            {
                result = this.appointmentService.DeleteAppointment(appointmentId);
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result;
        }

        [HttpGet("updateappointment")]
        public ActionResult<Cita> UpdateAppointment(Cita appointment)
        {
            Cita result = null;
            try
            {
                result = this.appointmentService.UpdateAppointment(appointment);
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result;
        }

        [HttpGet("updatepastappointmentsdata")]
        public ActionResult<bool> UpdatePastAppointmentsData()
        {
            bool result;
            try
            {
                result = this.appointmentService.UpdatePastAppointmentsData();
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result;
        }
    }
}
