namespace Uceme.Library.Services;

using System.Collections.Generic;
using Uceme.Model.Models;

public interface IHospitalService
{
    /// <summary>
    /// Gets a specific hospital by ID.
    /// </summary>
    /// <param name="hospitalId">The ID of the hospital to retrieve.</param>
    /// <returns>The hospital information.</returns>
    DatosProfesionales GetHospital(int hospitalId);

    /// <summary>
    /// Gets all active hospitals.
    /// </summary>
    /// <returns>List of active hospitals.</returns>
    IEnumerable<DatosProfesionales> GetHospitals();

    /// <summary>
    /// Creates a new hospital.
    /// </summary>
    /// <param name="hospital">The hospital information to create.</param>
    /// <returns>The created hospital information.</returns>
    DatosProfesionales CreateHospital(DatosProfesionales hospital);

    /// <summary>
    /// Updates an existing hospital.
    /// </summary>
    /// <param name="hospitalId">The ID of the hospital to update.</param>
    /// <param name="hospital">The updated hospital information.</param>
    /// <returns>The saved hospital information.</returns>
    DatosProfesionales UpdateHospital(int hospitalId, DatosProfesionales hospital);

    /// <summary>
    /// Soft deletes a hospital by setting its active status to false.
    /// </summary>
    /// <param name="hospitalId">The ID of the hospital to delete.</param>
    /// <returns>True if the hospital was deleted successfully.</returns>
    bool DeleteHospital(int hospitalId);

    /// <summary>
    /// Permanently deletes a hospital from the database.
    /// </summary>
    /// <param name="hospitalId">The ID of the hospital to delete.</param>
    /// <returns>True if the hospital was deleted successfully.</returns>
    bool HardDeleteHospital(int hospitalId);
}
