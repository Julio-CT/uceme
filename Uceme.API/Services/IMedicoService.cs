using System.Collections.Generic;
using Uceme.Model.Models.ClasesVista;

namespace Uceme.API.Services
{
    public interface IMedicoService
    {
        IEnumerable<MedicoMinVista> GetMedicoMinVista(bool hackOrder);
    }
}