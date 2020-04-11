namespace Uceme.API.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Uceme.Model.Models;
    using Uceme.Model.Models.ClasesVista;

    public class MedicoService : IMedicoService
    {
        protected UCEMEDbEntities DbContext = new UCEMEDbEntities();

        public IEnumerable<MedicoMinVista> GetMedicoMinVista(bool hackOrder)
        {
            var data = (from o in this.DbContext.Usuario
                        where o.idRol == 2
                        orderby o.display_order
                        select new MedicoMinVista
                        {
                            IdUsuario = o.idUsuario,
                            Nombre = o.nombre,
                            Apellidos = o.apellidos,
                            Foto = o.foto,
                            Titulo = o.Curriculum.Titulo,
                            Posicion = o.display_order
                        }).ToList();

            if (hackOrder)
            {
                for (var i = 0; i < data.Count; i++)
                {
                    data.ElementAt(i).Posicion = i;
                }
            }

            return data;
        }
    }
}
