namespace Uceme.API.Controllers;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Uceme.Library.Services;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;

// [Authorize]
[Route("api/[controller]")]
[ApiController]
public class ScheduleController : ControllerBase
{
    private readonly ILogger<ScheduleController> logger;
    private readonly ITurnoService turnoService;
    private readonly IHospitalService hospitalService;

    public ScheduleController(
        ILogger<ScheduleController> logger,
        ITurnoService turnoService,
        IHospitalService hospitalService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.turnoService = turnoService ?? throw new ArgumentNullException(nameof(turnoService));
        this.hospitalService = hospitalService ?? throw new ArgumentNullException(nameof(hospitalService));
    }

    // Hospital Endpoints

    /// <summary>
    /// Gets all active hospitals.
    /// </summary>
    /// <returns>List of hospitals.</returns>
    /// <response code="200">Returns the list of hospitals.</response>
    /// <response code="400">If there was a database error retrieving the hospitals.</response>
    /// <response code="500">If there was an unexpected error retrieving the hospitals.</response>
    [HttpGet("hospitals")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<DatosProfesionales>> GetHospitals()
    {
        try
        {
            this.logger.LogInformation("Getting all hospitals");
            var result = this.hospitalService.GetHospitals();

            if (!result.Any())
            {
                this.logger.LogWarning("No hospitals found");
                return this.Ok(Array.Empty<DatosProfesionales>());
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving hospitals");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve hospitals",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving hospitals");
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving hospitals",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    /// <summary>
    /// Gets a specific hospital by ID.
    /// </summary>
    /// <param name="hospitalId">The ID of the hospital to retrieve.</param>
    /// <returns>The hospital information.</returns>
    /// <response code="200">Returns the hospital information.</response>
    /// <response code="400">If there was a database error retrieving the hospital.</response>
    /// <response code="404">If the hospital was not found.</response>
    /// <response code="500">If there was an unexpected error retrieving the hospital.</response>
    [HttpGet("hospitals/{hospitalId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DatosProfesionales> GetHospital(int hospitalId)
    {
        try
        {
            this.logger.LogInformation("Getting hospital {HospitalId}", hospitalId);
            var result = this.hospitalService.GetHospital(hospitalId);

            if (result == null)
            {
                this.logger.LogWarning("Hospital {HospitalId} not found", hospitalId);
                return this.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Hospital with ID {hospitalId} not found",
                    Status = StatusCodes.Status404NotFound,
                });
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving hospital {HospitalId}", hospitalId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve hospital",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving hospital {HospitalId}", hospitalId);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving the hospital",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    /// <summary>
    /// Creates a new hospital.
    /// </summary>
    /// <param name="hospital">The hospital information to create.</param>
    /// <returns>The created hospital information.</returns>
    /// <response code="200">Returns the created hospital information.</response>
    /// <response code="400">If there was a database error creating the hospital.</response>
    /// <response code="500">If there was an unexpected error creating the hospital.</response>
    [HttpPost("hospitals")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DatosProfesionales> CreateHospital([FromBody] DatosProfesionales hospital)
    {
        if (hospital == null)
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Hospital data is required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        try
        {
            this.logger.LogInformation("Creating new hospital");
            var result = this.hospitalService.CreateHospital(hospital);
            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error creating hospital");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to create hospital",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error creating hospital");
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while creating the hospital",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    /// <summary>
    /// Updates an existing hospital.
    /// </summary>
    /// <param name="hospitalId">The ID of the hospital to update.</param>
    /// <param name="hospital">The updated hospital information.</param>
    /// <returns>The saved hospital information.</returns>
    /// <response code="200">Returns the updated hospital information.</response>
    /// <response code="400">If there was a database error updating the hospital.</response>
    /// <response code="404">If the hospital was not found.</response>
    /// <response code="500">If there was an unexpected error updating the hospital.</response>
    [HttpPut("hospitals/{hospitalId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DatosProfesionales> UpdateHospital(int hospitalId, [FromBody] DatosProfesionales hospital)
    {
        if (hospital == null)
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Hospital data is required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        try
        {
            this.logger.LogInformation("Updating hospital {HospitalId}", hospitalId);
            var result = this.hospitalService.UpdateHospital(hospitalId, hospital);
            return this.Ok(result);
        }
        catch (KeyNotFoundException)
        {
            this.logger.LogWarning("Hospital {HospitalId} not found", hospitalId);
            return this.NotFound(new ProblemDetails
            {
                Title = "Not Found",
                Detail = $"Hospital with ID {hospitalId} not found",
                Status = StatusCodes.Status404NotFound,
            });
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error updating hospital {HospitalId}", hospitalId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to update hospital",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error updating hospital {HospitalId}", hospitalId);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while updating the hospital",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    /// <summary>
    /// Deletes a hospital.
    /// </summary>
    /// <param name="hospitalId">The ID of the hospital to delete.</param>
    /// <returns>True if the hospital was deleted successfully.</returns>
    /// <response code="200">Returns true if the hospital was deleted successfully.</response>
    /// <response code="400">If there was a database error deleting the hospital.</response>
    /// <response code="404">If the hospital was not found.</response>
    /// <response code="500">If there was an unexpected error deleting the hospital.</response>
    [HttpDelete("hospitals/{hospitalId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> DeleteHospital(int hospitalId)
    {
        try
        {
            this.logger.LogInformation("Deleting hospital {HospitalId}", hospitalId);
            var result = this.hospitalService.DeleteHospital(hospitalId);
            return this.Ok(result);
        }
        catch (KeyNotFoundException)
        {
            this.logger.LogWarning("Hospital {HospitalId} not found", hospitalId);
            return this.NotFound(new ProblemDetails
            {
                Title = "Not Found",
                Detail = $"Hospital with ID {hospitalId} not found",
                Status = StatusCodes.Status404NotFound,
            });
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error deleting hospital {HospitalId}", hospitalId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to delete hospital",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error deleting hospital {HospitalId}", hospitalId);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while deleting the hospital",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    // Turn Endpoints

    /// <summary>
    /// Gets all available turns.
    /// </summary>
    /// <returns>List of turns.</returns>
    /// <response code="200">Returns the list of turns.</response>
    /// <response code="400">If there was a database error retrieving the turns.</response>
    /// <response code="500">If there was an unexpected error retrieving the turns.</response>
    [HttpGet("turns")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Turn>> GetTurns()
    {
        try
        {
            this.logger.LogInformation("Getting all turns");
            var result = this.turnoService.GetTurnos();

            if (!result.Any())
            {
                this.logger.LogWarning("No turns found");
                return this.Ok(Array.Empty<Turn>());
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving turns");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve turns",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving turns");
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving turns",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    /// <summary>
    /// Gets a specific turn by ID.
    /// </summary>
    /// <param name="turnId">The ID of the turn to retrieve.</param>
    /// <returns>The turn information.</returns>
    /// <response code="200">Returns the turn information.</response>
    /// <response code="400">If there was a database error retrieving the turn.</response>
    /// <response code="404">If the turn was not found.</response>
    /// <response code="500">If there was an unexpected error retrieving the turn.</response>
    [HttpGet("turns/{turnId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Turn> GetTurn(int turnId)
    {
        try
        {
            this.logger.LogInformation("Getting turn {TurnId}", turnId);
            var result = this.turnoService.GetTurno(turnId);

            if (result == null)
            {
                this.logger.LogWarning("Turn {TurnId} not found", turnId);
                return this.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Turn with ID {turnId} not found",
                    Status = StatusCodes.Status404NotFound,
                });
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving turn {TurnId}", turnId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve turn",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving turn {TurnId}", turnId);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving the turn",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    /// <summary>
    /// Gets all turns for a specific hospital.
    /// </summary>
    /// <param name="hospitalId">The ID of the hospital.</param>
    /// <returns>List of turns for the hospital.</returns>
    /// <response code="200">Returns the list of turns for the hospital.</response>
    /// <response code="400">If there was a database error retrieving the turns.</response>
    /// <response code="500">If there was an unexpected error retrieving the turns.</response>
    [HttpGet("hospitals/{hospitalId}/turns")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Turn>> GetTurnsByHospital(int hospitalId)
    {
        try
        {
            this.logger.LogInformation("Getting turns for hospital {HospitalId}", hospitalId);
            var result = this.turnoService.GetTurnosByHospital(hospitalId);

            if (!result.Any())
            {
                this.logger.LogWarning("No turns found for hospital {HospitalId}", hospitalId);
                return this.Ok(Array.Empty<Turn>());
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving turns for hospital {HospitalId}", hospitalId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve turns for hospital",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving turns for hospital {HospitalId}", hospitalId);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving turns for hospital",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    /// <summary>
    /// Creates a new turn.
    /// </summary>
    /// <param name="turn">The turn information to create.</param>
    /// <returns>The created turn information.</returns>
    /// <response code="200">Returns the created turn information.</response>
    /// <response code="400">If there was a database error creating the turn.</response>
    /// <response code="500">If there was an unexpected error creating the turn.</response>
    [HttpPost("turns")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Turn> CreateTurn([FromBody] Turn turn)
    {
        if (turn == null)
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Turn data is required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        try
        {
            this.logger.LogInformation("Creating new turn");
            var result = this.turnoService.CreateTurno(turn);
            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error creating turn");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to create turn",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error creating turn");
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while creating the turn",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    /// <summary>
    /// Updates an existing turn.
    /// </summary>
    /// <param name="turnId">The ID of the turn to update.</param>
    /// <param name="turn">The updated turn information.</param>
    /// <returns>The updated turnos information.</returns>
    /// <response code="200">Returns the updated turn information.</response>
    /// <response code="400">If there was a database error updating the turn.</response>
    /// <response code="404">If the turn was not found.</response>
    /// <response code="500">If there was an unexpected error updating the turn.</response>
    [HttpPut("turns/{turnId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Turn> UpdateTurn(int turnId, [FromBody] Turn turn)
    {
        if (turn == null)
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Turn data is required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        try
        {
            this.logger.LogInformation("Updating turn {TurnId}", turnId);
            var result = this.turnoService.UpdateTurno(turnId, turn);
            return this.Ok(result);
        }
        catch (KeyNotFoundException)
        {
            this.logger.LogWarning("Turn {TurnId} not found", turnId);
            return this.NotFound(new ProblemDetails
            {
                Title = "Not Found",
                Detail = $"Turn with ID {turnId} not found",
                Status = StatusCodes.Status404NotFound,
            });
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error updating turn {TurnId}", turnId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to update turn",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error updating turn {TurnId}", turnId);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while updating the turn",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    /// <summary>
    /// Deletes a turn.
    /// </summary>
    /// <param name="turnId">The ID of the turn to delete.</param>
    /// <returns>True if the turn was deleted successfully.</returns>
    /// <response code="200">Returns true if the turn was deleted successfully.</response>
    /// <response code="400">If there was a database error deleting the turn.</response>
    /// <response code="404">If the turn was not found.</response>
    /// <response code="500">If there was an unexpected error deleting the turn.</response>
    [HttpDelete("turns/{turnId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> DeleteTurn(int turnId)
    {
        try
        {
            this.logger.LogInformation("Deleting turn {TurnId}", turnId);
            var result = this.turnoService.DeleteTurno(turnId);
            return this.Ok(result);
        }
        catch (KeyNotFoundException)
        {
            this.logger.LogWarning("Turn {TurnId} not found", turnId);
            return this.NotFound(new ProblemDetails
            {
                Title = "Not Found",
                Detail = $"Turn with ID {turnId} not found",
                Status = StatusCodes.Status404NotFound,
            });
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error deleting turn {TurnId}", turnId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to delete turn",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error deleting turn {TurnId}", turnId);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while deleting the turn",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }
}
