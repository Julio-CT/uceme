namespace Uceme.API.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Logging;
    using Uceme.API.Data;
    using Uceme.Model.Models;

    public class FotosService : IFotosService
    {
        private readonly ILogger<FotosService> _logger;

        public ApplicationDbContext DbContext { get; }

        public FotosService(ILogger<FotosService> logger, ApplicationDbContext context)
        {
            this._logger = logger;
            this.DbContext = context;
        }
        public IEnumerable<Fotos> GetFotos()
        {
            var listaFotos = this.DbContext.Fotos.Where(o => o.destacada.Value);

            return listaFotos;
        }
    }
}
