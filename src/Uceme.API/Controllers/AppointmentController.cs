namespace Uceme.API.Controllers;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Uceme.Library.Services;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;

[Authorize]
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
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
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
            this.logger.LogError("error getting days");
            return this.BadRequest();
        }

        return result.ToList();
    }

    [HttpPost("gethours")]
    [AllowAnonymous]
    public ActionResult<AppointmentHoursResponse> GetHours([FromBody] AppointmentHoursRequest appointmentHoursRequest)
    {
        if (appointmentHoursRequest is null)
        {
            return this.BadRequest($"'{nameof(appointmentHoursRequest)}' cannot be null or empty.");
        }

        AppointmentHoursResponse result = new AppointmentHoursResponse();
        try
        {
            result.Hours = this.appointmentService.GetHours(appointmentHoursRequest);
        }
        catch (DataException)
        {
            this.logger.LogError("error getting hours");
            return this.BadRequest();
        }

        return result;
    }

    [HttpPost("addappointment")]
    [AllowAnonymous]
    public async Task<ActionResult<bool>> AddAppointmentAsync([FromBody] AppointmentRequest appointmentRequest)
    {
        if (appointmentRequest is null)
        {
            return this.BadRequest($"'{nameof(appointmentRequest)}' cannot be null or empty.");
        }

        bool result;
        try
        {
            result = await this.appointmentService.AddAppointmentAsync(appointmentRequest).ConfigureAwait(false);
        }
        catch (DataException)
        {
            this.logger.LogError("error adding appointment");
            return this.BadRequest();
        }

        return result;
    }

    [HttpGet("appointmentlist")]
    public ActionResult<IEnumerable<Appointment>> AppointmentList()
    {
        List<Appointment> result = new List<Appointment>();
        try
        {
            IEnumerable<Appointment>? appointments = this.appointmentService.GetAppointments();

            if (appointments != null)
            {
                result.AddRange(appointments);
            }
        }
        catch (DataException)
        {
            this.logger.LogError("error getting appointment list");
            return this.BadRequest();
        }

        return result;
    }

    [HttpGet("closeappointmentlist")]
    public ActionResult<IEnumerable<Appointment>> CloseAppointmentList()
    {
        List<Appointment> result = new List<Appointment>();
        try
        {
            result.AddRange(this.appointmentService.GetCloseAppointments());
        }
        catch (DataException)
        {
            this.logger.LogError("error getting the close appointment list");
            return this.BadRequest();
        }

        return result;
    }

    [HttpGet("appointmenteventslist")]
    public ActionResult<IEnumerable<CalendarEvent>> AppointmentEventsList()
    {
        List<CalendarEvent> result = new List<CalendarEvent>();
        try
        {
            IEnumerable<CalendarEvent>? appointments = this.appointmentService.GetAppointmentsEvents();

            if (appointments != null)
            {
                result.AddRange(appointments);
            }
        }
        catch (DataException)
        {
            this.logger.LogError("error getting appointment list");
            return this.BadRequest();
        }

        return result;
    }

    [HttpGet("getappointment")]
    public ActionResult<Appointment> GetAppointment(int appointmentId)
    {
        Appointment result;
        try
        {
            result = this.appointmentService.GetAppointment(appointmentId);
        }
        catch (DataException)
        {
            this.logger.LogError("error getting appointment");
            return this.BadRequest();
        }

        return result;
    }

    [HttpGet("deleteappointment")]
    public ActionResult<bool> DeleteAppointment(int appointmentId)
    {
        bool result;
        try
        {
            result = this.appointmentService.DeleteAppointment(appointmentId);
        }
        catch (DataException)
        {
            this.logger.LogError("error deleting appointment");
            return this.BadRequest();
        }

        return result;
    }

    [HttpGet("updateappointment")]
    public ActionResult<Appointment> UpdateAppointment(Cita appointment)
    {
        if (appointment is null)
        {
            return this.BadRequest($"'{nameof(appointment)}' cannot be null or empty.");
        }

        Appointment result;
        try
        {
            result = this.appointmentService.UpdateAppointment(appointment);
        }
        catch (DataException)
        {
            this.logger.LogError("error updating appointment");
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
            this.logger.LogError("error updating past appointment");
            return this.BadRequest();
        }

        return result;
    }
}
