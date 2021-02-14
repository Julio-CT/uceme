namespace Uceme.API.Services
{
    using System.Collections.Generic;
    using Uceme.Model.Models;

    public interface IHospitalService
    {
        DatosProfesionales GetHospital(int hospitalId);

        IEnumerable<DatosProfesionales> GetHospitals();
    }
}