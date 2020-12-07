namespace Uceme.API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Uceme.API.Data;
    using Uceme.Model.Models;

    public class MedicoService : IMedicoService
    {
        private readonly ILogger<FotosService> logger;

        public ApplicationDbContext DbContext { get; }

        public MedicoService(ILogger<FotosService> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.DbContext = context;
        }

        public IEnumerable<Usuario> GetMedicoMinVista(bool hackOrder)
        {
            try
            {
                var data = this.DbContext.Usuario.Where(us => us.idRol == 2).OrderBy(o => o.display_order).Select(o => new Usuario
                {
                    idUsuario = o.idUsuario,
                    nombre = o.nombre,
                    apellidos = o.apellidos,
                    foto = o.foto,
                });
                this.logger.LogInformation($"retrieved {data.Count()} items");

                return data;
            }
            catch (Exception e)
            {
                throw new DataException("Error retrieving MedicoMinVista", e);
            }
        }
    }
}
