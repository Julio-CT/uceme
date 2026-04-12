using System.Collections.Generic;
using Uceme.Model.Models;

namespace Uceme.Library.Services;

public interface IMedicoService
{
    IEnumerable<Usuario> GetMedicoMinVista(bool hackOrder);
}
