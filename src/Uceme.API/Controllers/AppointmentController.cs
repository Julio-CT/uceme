namespace Uceme.API.Controllers;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

    [HttpGet("days/{hospitalId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<int>> GetDays(int hospitalId)
    {
        try
        {
            var result = this.appointmentService.GetDays(hospitalId);
            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(
                ex,
                "Error retrieving available days for hospital {HospitalId}",
                hospitalId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve available days",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Unexpected error retrieving days for hospital {HospitalId}",
                hospitalId);
            return this.StatusCode(
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred while retrieving available days",
                    Status = StatusCodes.Status500InternalServerError,
                });
        }
    }

    [HttpGet("hours")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<AppointmentHoursResponse> GetHours(
        [FromQuery] int weekDay,
        [FromQuery] string hospitalId,
        [FromQuery] int day,
        [FromQuery] int month,
        [FromQuery] int year)
    {
        if (string.IsNullOrEmpty(hospitalId))
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Hospital ID is required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        try
        {
            var appointmentHoursRequest = new AppointmentHoursRequest
            {
                WeekDay = weekDay,
                HospitalId = hospitalId,
                Day = day,
                Month = month,
                Year = year,
            };

            var hours = this.appointmentService.GetHours(appointmentHoursRequest);
            return this.Ok(new AppointmentHoursResponse { Hours = hours });
        }
        catch (DataException ex)
        {
            this.logger.LogError(
                ex,
                "Error retrieving available hours for hospital {HospitalId} on {Day}/{Month}/{Year}",
                hospitalId,
                day,
                month,
                year);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve available hours",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Unexpected error retrieving hours for hospital {HospitalId} on {Day}/{Month}/{Year}",
                hospitalId,
                day,
                month,
                year);
            return this.StatusCode(
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred while retrieving available hours",
                    Status = StatusCodes.Status500InternalServerError,
                });
        }
    }

    [HttpPost("addappointment")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> AddAppointmentAsync([FromBody] AppointmentRequest appointmentRequest)
    {
        if (appointmentRequest is null)
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Appointment request data is required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        try
        {
            var result = await this.appointmentService.AddAppointmentAsync(appointmentRequest)
                .ConfigureAwait(false);
            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(
                ex,
                "Error adding appointment for {Name} on {Day}/{Month}/{Year}",
                appointmentRequest.Name,
                appointmentRequest.Day,
                appointmentRequest.Month,
                appointmentRequest.Year);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to add appointment",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Unexpected error adding appointment for {Name} on {Day}/{Month}/{Year}",
                appointmentRequest.Name,
                appointmentRequest.Day,
                appointmentRequest.Month,
                appointmentRequest.Year);
            return this.StatusCode(
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred while adding the appointment",
                    Status = StatusCodes.Status500InternalServerError,
                });
        }
    }

    [HttpGet("appointmentlist")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Appointment>> AppointmentList()
    {
        try
        {
            var result = this.appointmentService.GetAppointments();
            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(
                ex,
                "Error retrieving appointments list");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve appointments list",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Unexpected error retrieving appointments list");
            return this.StatusCode(
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred while retrieving appointments list",
                    Status = StatusCodes.Status500InternalServerError,
                });
        }
    }

    [HttpGet("closeappointmentlist")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Appointment>> CloseAppointmentList()
    {
        try
        {
            var result = this.appointmentService.GetCloseAppointments();
            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(
                ex,
                "Error retrieving close appointments list");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve close appointments list",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Unexpected error retrieving close appointments list");
            return this.StatusCode(
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred while retrieving close appointments list",
                    Status = StatusCodes.Status500InternalServerError,
                });
        }
    }

    [HttpGet("appointmenteventslist")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<CalendarEvent>> AppointmentEventsList()
    {
        try
        {
            var events = this.appointmentService.GetAppointmentsEvents();
            return this.Ok(events ?? Array.Empty<CalendarEvent>());
        }
        catch (DataException ex)
        {
            this.logger.LogError(
                ex,
                "Error retrieving appointment events list");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve appointment events",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Unexpected error retrieving appointment events list");
            return this.StatusCode(
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred while retrieving appointment events",
                    Status = StatusCodes.Status500InternalServerError,
                });
        }
    }

    [HttpGet("getappointment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Appointment> GetAppointment(int appointmentId)
    {
        try
        {
            var result = this.appointmentService.GetAppointment(appointmentId);
            if (result == null)
            {
                return this.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Appointment with ID {appointmentId} not found",
                    Status = StatusCodes.Status404NotFound,
                });
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(
                ex,
                "Error retrieving appointment {AppointmentId}",
                appointmentId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve appointment",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Unexpected error retrieving appointment {AppointmentId}",
                appointmentId);
            return this.StatusCode(
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred while retrieving the appointment",
                    Status = StatusCodes.Status500InternalServerError,
                });
        }
    }

    [HttpDelete("{appointmentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> DeleteAppointment(int appointmentId)
    {
        try
        {
            var result = this.appointmentService.DeleteAppointment(appointmentId);
            if (!result)
            {
                return this.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Appointment with ID {appointmentId} not found",
                    Status = StatusCodes.Status404NotFound,
                });
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(
                ex,
                "Error deleting appointment {AppointmentId}",
                appointmentId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to delete appointment",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Unexpected error deleting appointment {AppointmentId}",
                appointmentId);
            return this.StatusCode(
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred while deleting the appointment",
                    Status = StatusCodes.Status500InternalServerError,
                });
        }
    }

    [HttpPut("{appointmentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Appointment> UpdateAppointment([FromBody] Cita appointment)
    {
        if (appointment is null)
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Appointment data is required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        try
        {
            var result = this.appointmentService.UpdateAppointment(appointment);
            if (result == null)
            {
                return this.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Appointment with ID {appointment.idCita} not found",
                    Status = StatusCodes.Status404NotFound,
                });
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(
                ex,
                "Error updating appointment {AppointmentId}",
                appointment.idCita);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to update appointment",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Unexpected error updating appointment {AppointmentId}",
                appointment.idCita);
            return this.StatusCode(
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred while updating the appointment",
                    Status = StatusCodes.Status500InternalServerError,
                });
        }
    }

    [HttpPut("past-appointments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> UpdatePastAppointmentsData()
    {
        try
        {
            var result = this.appointmentService.UpdatePastAppointmentsData();
            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(
                ex,
                "Error updating past appointments data");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to update past appointments data",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Unexpected error updating past appointments data");
            return this.StatusCode(
                StatusCodes.Status500InternalServerError,
                new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred while updating past appointments data",
                    Status = StatusCodes.Status500InternalServerError,
                });
        }
    }
}
