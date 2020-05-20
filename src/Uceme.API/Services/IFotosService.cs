namespace Uceme.API.Services
{
    using System.Collections.Generic;
    using Uceme.Model.Models;

    public interface IFotosService
    {
        IEnumerable<Fotos> GetFotos();
    }
}