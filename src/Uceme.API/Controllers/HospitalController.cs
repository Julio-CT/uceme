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
using Uceme.Model.Models;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class HospitalController : Controller
{
    private readonly ILogger<HospitalController> logger;
    private readonly IHospitalService hospitalService;

    public HospitalController(
        ILogger<HospitalController> logger,
        IHospitalService hospitalService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.hospitalService = hospitalService ?? throw new ArgumentNullException(nameof(hospitalService));
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<DatosProfesionales>> GetHospitals()
    {
        try
        {
            var result = this.hospitalService.GetHospitals();
            return this.Ok(result.ToList());
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving hospitals list");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve hospitals list",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving hospitals list");
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving hospitals list",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    [HttpGet("gethospital")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DatosProfesionales> GetHospital(int hostpitalId)
    {
        try
        {
            var result = this.hospitalService.GetHospital(hostpitalId);
            if (result == null)
            {
                return this.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Hospital with ID {hostpitalId} not found",
                    Status = StatusCodes.Status404NotFound,
                });
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving hospital {HospitalId}", hostpitalId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve hospital information",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving hospital {HospitalId}", hostpitalId);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving hospital information",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }
}
