namespace Uceme.API.Controllers;

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Uceme.Model.Settings;

[Route("api/[controller]")]
[ApiController]
public class SettingsController : Controller
{
    private readonly ILogger<SettingsController> logger;
    private readonly IOptions<AppSettings> configuration;

    public SettingsController(
        ILogger<SettingsController> logger,
        IOptions<AppSettings> configuration)
    {
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("getsettings")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Dictionary<string, object>> GetSettings()
    {
        try
        {
            var result = new Dictionary<string, object>
            {
                ["telephone"] = this.configuration?.Value?.Telephone ?? string.Empty,
                ["contactEmail"] = this.configuration?.Value?.ContactEmail ?? string.Empty,
                ["address"] = this.configuration?.Value?.Address ?? string.Empty,
            };

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving application settings");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Settings Error",
                Detail = "Unable to retrieve application settings",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving application settings");
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving application settings",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }
}
