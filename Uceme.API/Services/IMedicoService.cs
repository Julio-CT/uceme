using System.Collections.Generic;
using Uceme.Model.Models;

namespace Uceme.API.Services
{
    public interface IMedicoService
    {
        IEnumerable<Usuario> GetMedicoMinVista(bool hackOrder);
    }
}