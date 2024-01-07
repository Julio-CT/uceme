namespace Uceme.API.Controllers;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Uceme.Library.Services;
using Uceme.Model.Models;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ScheduleController : Controller
{
    private readonly ILogger<ScheduleController> logger;
    private readonly IScheduleService scheduleService;

    public ScheduleController(
        ILogger<ScheduleController> logger,
        IScheduleService scheduleService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.scheduleService = scheduleService ?? throw new ArgumentNullException(nameof(scheduleService));
    }

    [HttpGet("getturns")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<Turno>> GetTurns()
    {
        IEnumerable<Turno> result;
        try
        {
            result = this.scheduleService.GetTurns();
        }
        catch (DataException)
        {
            this.logger.LogError("error getting days");
            return this.BadRequest();
        }

        return result.ToList();
    }
}
