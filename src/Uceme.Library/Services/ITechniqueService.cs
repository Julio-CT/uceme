namespace Uceme.Library.Services
{
    using System.Collections.Generic;
    using Uceme.Model.DataContracts;
    using Uceme.Model.Models;

    public interface ITechniqueService
    {
        Tecnica GetTechnique(int techniqueId);

        IEnumerable<Tecnica> GetTechniques();

        bool DeleteTechnique(int techId);

        Tecnica UpdateTechine(Tecnica post);

        bool UpdateTechnique(TechniqueRequest postRequest);

        bool AddTechnique(TechniqueRequest postRequest);

        string GetNextTechImage();
    }
}
