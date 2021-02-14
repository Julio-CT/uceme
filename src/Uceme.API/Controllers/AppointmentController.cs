namespace Uceme.API.Controllers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Uceme.API.Services;
    using Uceme.Model.DataContracts;

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

        [HttpGet("getdays")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<int>> GetDays(int hospitalId)
        {
            IEnumerable<int> result;
            try
            {
                result = this.appointmentService.GetDays(hospitalId);
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result.ToList();
        }

        [HttpPost("gethours")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<string>> GetHours([FromBody] AppointmentHoursRequest appointmentHoursRequest)
        {
            IEnumerable<string> result;
            try
            {
                result = this.appointmentService.GetHours(appointmentHoursRequest);
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result.ToList();
        }

        [HttpPost("addappointment")]
        [AllowAnonymous]
        public ActionResult<bool> AddApointment([FromBody] AppointmentRequest appointmentRequest)
        {
            bool result;
            try
            {
                result = this.appointmentService.AddAppointment(appointmentRequest);
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result;
        }
    }
}
