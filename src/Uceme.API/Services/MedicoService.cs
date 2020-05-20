namespace Uceme.API.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Uceme.API.Data;
    using Uceme.Model.Models;

    public class MedicoService : IMedicoService
    {
        private readonly ILogger<FotosService> _logger;

        public ApplicationDbContext DbContext { get; }

        public MedicoService(ILogger<FotosService> logger, ApplicationDbContext context)
        {
            this._logger = logger;
            this.DbContext = context;
        }

        public IEnumerable<Usuario> GetMedicoMinVista(bool hackOrder)
        {
            var data = (from o in this.DbContext.Usuario
                        where o.idRol == 2
                        orderby o.display_order
                        select new Usuario
                        {
                            idUsuario = o.idUsuario,
                            nombre = o.nombre,
                            apellidos = o.apellidos,
                            foto = o.foto,
                        }).ToList();

            return data;
        }
    }
}
