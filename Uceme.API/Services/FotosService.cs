namespace Uceme.API.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Uceme.Model.Models;
    using Uceme.Model.Models.ClasesVista;

    public class FotosService : IFotosService
    {
        protected UCEMEDbEntities DbContext = new UCEMEDbEntities();

        public IEnumerable<FotosVista> GetFotos()
        {
            var listaFotos = (from o in this.DbContext.Fotos
                              where o.destacada.Value
                              select new FotosVista
                              {
                                  IdFoto = o.idFoto,
                                  Nombre = o.nombre,
                                  Texto = o.texto
                              }).ToList();

            return listaFotos;
        }
    }
}
