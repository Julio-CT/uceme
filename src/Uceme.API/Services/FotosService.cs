namespace Uceme.API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Uceme.Model.Data;
    using Uceme.Model.Models;

    public class FotosService : IFotosService
    {
        private readonly ILogger<FotosService> logger;

        private readonly ApplicationDbContext dbContext;

        public FotosService(ILogger<FotosService> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.dbContext = context;
        }

        public IEnumerable<Fotos> GetFotos()
        {
            try
            {
                var listaFotos = this.dbContext.Fotos.Where(o => o.destacada.Value);
                this.logger.LogInformation($"retrieved {listaFotos.Count()} items");

                return listaFotos;
            }
            catch (Exception e)
            {
                throw new DataException("Error retrieving fotos", e);
            }
        }
    }
}
