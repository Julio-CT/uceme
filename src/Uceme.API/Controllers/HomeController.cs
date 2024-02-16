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
    public ActionResult<IEnumerable<Usuario>> GetMedicoMinVista()
    {
        IEnumerable<Usuario> result;
        try
        {
            result = this.medicoService.GetMedicoMinVista(true);
        }
        catch (DataException)
        {
            this.logger.LogError("error getting doctor");
            return this.BadRequest();
        }

        return result.ToList();
    }

    [HttpGet("mostrarfotos")]
    [AllowAnonymous]
    public ActionResult<IEnumerable<Foto>> MostrarFotos()
    {
        IEnumerable<Foto> listaFotos;
        try
        {
            listaFotos = this.fotosService.GetFotos();
        }
        catch (DataException)
        {
            this.logger.LogError("error getting picture");
            return this.BadRequest();
        }

        return listaFotos.ToList();
    }
}
