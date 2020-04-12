using System.Collections.Generic;
using Uceme.Model.Models;

namespace Uceme.API.Services
{
    public interface IFotosService
    {
        IEnumerable<Fotos> GetFotos();
    }
}