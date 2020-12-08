namespace Uceme.API.Controllers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Uceme.API.Services;
    using Uceme.Model.Models;

    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
    
        private readonly IMedicoService medicoService;
    
        private readonly IFotosService fotosService;

        public HomeController(IMedicoService medicoService, IFotosService fotosService, ILogger<HomeController> logger)
        {
            this.medicoService = medicoService;
            this.fotosService = fotosService;
            this.logger = logger;
        }

        [HttpGet("[controller]/getmedicominvista")]
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
                return this.BadRequest();
            }

            return result.ToList();
        }

        [HttpGet("[controller]/mostrarfotos")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Fotos>> MostrarFotos()
        {
            IEnumerable<Fotos> listaFotos;
            try
            {
                listaFotos = this.fotosService.GetFotos();
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return listaFotos.ToList();
        }
    }
}