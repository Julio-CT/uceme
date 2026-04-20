using System.Collections.Generic;
using Uceme.Model.Models;

namespace Uceme.Library.Services;

public interface IFotosService
{
    IEnumerable<Foto> GetFotos();
}
