using System.Collections.Generic;
using Uceme.Model.Models.ClasesVista;

namespace Uceme.API.Services
{
    public interface IFotosService
    {
        IEnumerable<FotosVista> GetFotos();
    }
}