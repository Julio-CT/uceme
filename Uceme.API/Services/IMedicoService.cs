namespace Uceme.API.Services
{
    using System.Collections.Generic;
    using Uceme.Model.Models;

    public interface IMedicoService
    {
        IEnumerable<Usuario> GetMedicoMinVista(bool hackOrder);
    }
}