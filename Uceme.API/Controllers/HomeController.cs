namespace Uceme.API.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Uceme.API.Services;
    using Uceme.Model.Models.ClasesVista;

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
        public ActionResult<IEnumerable<MedicoMinVista>> GetMedicoMinVista()
        {
            IEnumerable<MedicoMinVista> result;
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
        public ActionResult<IEnumerable<FotosVista>> MostrarFotos()
        {
            IEnumerable<FotosVista> listaFotos;
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