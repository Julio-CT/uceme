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
public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;
    private readonly IMedicoService medicoService;
    private readonly IFotosService fotosService;

    public HomeController(
        IMedicoService medicoService,
        IFotosService fotosService,
        ILogger<HomeController> logger)
    {
        this.medicoService = medicoService ?? throw new ArgumentNullException(nameof(medicoService));
        this.fotosService = fotosService ?? throw new ArgumentNullException(nameof(fotosService));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("getmedicominvista")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Usuario>> GetMedicoMinVista()
    {
        try
        {
            var result = this.medicoService.GetMedicoMinVista(true);
            return this.Ok(result.ToList());
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving doctor information");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve doctor information",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving doctor information");
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving doctor information",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    [HttpGet("mostrarfotos")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Foto>> MostrarFotos()
    {
        try
        {
            var listaFotos = this.fotosService.GetFotos();
            return this.Ok(listaFotos.ToList());
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving photos");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve photos",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving photos");
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving photos",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }
}
