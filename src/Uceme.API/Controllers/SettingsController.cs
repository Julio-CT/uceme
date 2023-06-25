namespace Uceme.API.Controllers;

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Authorization;
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
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        this.configuration = configuration;
        this.logger = logger;
    }

    [HttpGet("getsettings")]
    [AllowAnonymous]
    public ActionResult<Dictionary<string, object>> GetSettings()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        try
        {
            result["telephone"] = this.configuration?.Value?.Telephone ?? string.Empty;
            result["contactEmail"] = this.configuration?.Value?.ContactEmail ?? string.Empty;
            result["address"] = this.configuration?.Value?.Address ?? string.Empty;
        }
        catch (DataException)
        {
            this.logger.LogError("error returning settings");
            return this.BadRequest();
        }

        return result;
    }
}
