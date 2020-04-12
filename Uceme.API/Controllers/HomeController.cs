namespace Uceme.API.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Uceme.API.Services;
    using Uceme.Model.Models;

    public class HomeController : SuperController
    {
        private readonly IMedicoService medicoService;
        private readonly IFotosService fotosService;

        public HomeController(IMedicoService medicoService, IFotosService fotosService)
        {
            this.medicoService = medicoService;
            this.fotosService = fotosService;
        }

        [HttpGet]
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

        [HttpGet]
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