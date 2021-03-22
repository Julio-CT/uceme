namespace Uceme.API.Controllers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Uceme.Library.Services;
    using Uceme.Model.DataContracts;

    [Route("api/[controller]")]
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
        public ActionResult<AppointmentHoursResponse> GetHours([FromBody] AppointmentHoursRequest appointmentHoursRequest)
        {
            var result = new AppointmentHoursResponse();
            try
            {
                result.Hours = this.appointmentService.GetHours(appointmentHoursRequest);

            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result;
        }

        [HttpPost("addappointment")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> AddApointmentAsync([FromBody] AppointmentRequest appointmentRequest)
        {
            bool result;
            try
            {
                result = await this.appointmentService.AddAppointmentAsync(appointmentRequest).ConfigureAwait(false);
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result;
        }
    }
}
