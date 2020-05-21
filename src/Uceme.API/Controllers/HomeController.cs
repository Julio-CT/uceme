namespace Uceme.API.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Uceme.API.Services;
    using Uceme.Model.Models;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMedicoService medicoService;
        private readonly IFotosService fotosService;

        public HomeController(IMedicoService medicoService, IFotosService fotosService, ILogger<HomeController> logger)
        {
            this.medicoService = medicoService;
            this.fotosService = fotosService;
            this._logger = logger;
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
            catch
            {
                return this.BadRequest();
            }

            return result.ToList();
        }

        [HttpGet("mostrarfotos")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Fotos>> MostrarFotos()
        {
            IEnumerable<Fotos> listaFotos;
            try
            {
                listaFotos = this.fotosService.GetFotos();
            }
            catch
            {
                return this.BadRequest();
            }

            return listaFotos.ToList();
        }
    }
}