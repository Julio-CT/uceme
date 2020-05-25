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
