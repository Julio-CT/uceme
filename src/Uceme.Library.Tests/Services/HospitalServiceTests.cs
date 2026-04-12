using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.Library.Services;
using Uceme.Model.Data;
using Uceme.Model.Models;

namespace Uceme.Library.Tests.Services;

[TestClass]
public class HospitalServiceTests
{
    private Mock<ILogger<HospitalService>> mockLogger;
    private ApplicationDbContext context;
    private HospitalService hospitalService;

    [TestInitialize]
    public void Setup()
    {
        this.mockLogger = new Mock<ILogger<HospitalService>>();

        // Create in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        this.context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());

        this.hospitalService = new HospitalService(this.mockLogger.Object, this.context);
    }

    [TestMethod]
    public void GetHospitals_ShouldReturnHospitals_WhenHospitalsExist()
    {
        // Arrange
        var hospitals = new List<DatosProfesionales>
        {
            new DatosProfesionales { activo = true },
            new DatosProfesionales { activo = true },
            new DatosProfesionales { activo = false },
        };

        this.context.DatosProfesionales.AddRange(hospitals);
        this.context.SaveChanges();

        // Act
        var result = this.hospitalService.GetHospitals();

        // Assert
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void GetHospitals_ShouldThrowDataException_WhenExceptionIsThrown()
    {
        // Arrange
        this.context.Dispose();

        // Act
        this.hospitalService.GetHospitals();
    }

    [TestMethod]
    public void GetHospital_ShouldReturnHospital_WhenHospitalExists()
    {
        // Arrange
        var hospitalId = 1;
        var hospitals = new List<DatosProfesionales>
        {
            new DatosProfesionales { idDatosPro = hospitalId, activo = true,  },
        };

        this.context.DatosProfesionales.AddRange(hospitals);
        this.context.SaveChanges();

        // Act
        var result = this.hospitalService.GetHospital(hospitalId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(hospitalId, result.idDatosPro);
    }

    [TestMethod]
    public void CreateHospital_ShouldCreateHospital_WhenValidDataProvided()
    {
        // Arrange
        var hospital = new DatosProfesionales
        {
            nombre = "Test Hospital",
            direccion = "Test Address",
            telefono = "123456789",
            email = "test@hospital.com",
        };

        // Act
        var result = this.hospitalService.CreateHospital(hospital);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.activo);
        Assert.AreEqual(hospital.nombre, result.nombre);
        Assert.AreEqual(hospital.direccion, result.direccion);
        Assert.AreEqual(hospital.telefono, result.telefono);
        Assert.AreEqual(hospital.email, result.email);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void CreateHospital_ShouldThrowArgumentNullException_WhenNullHospitalProvided()
    {
        // Act
        this.hospitalService.CreateHospital(null);
    }

    [TestMethod]
    public void UpdateHospital_ShouldUpdateHospital_WhenValidDataProvided()
    {
        // Arrange
        var hospitalId = 1;
        var existingHospital = new DatosProfesionales
        {
            idDatosPro = hospitalId,
            nombre = "Original Hospital",
            direccion = "Original Address",
            telefono = "123456789",
            email = "original@hospital.com",
            activo = true,
        };

        this.context.DatosProfesionales.Add(existingHospital);
        this.context.SaveChanges();

        var updatedHospital = new DatosProfesionales
        {
            nombre = "Updated Hospital",
            direccion = "Updated Address",
            telefono = "987654321",
            email = "updated@hospital.com",
            activo = true,
        };

        // Act
        var result = this.hospitalService.UpdateHospital(hospitalId, updatedHospital);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(hospitalId, result.idDatosPro);
        Assert.AreEqual(updatedHospital.nombre, result.nombre);
        Assert.AreEqual(existingHospital.direccion, result.direccion);
        Assert.AreEqual(existingHospital.telefono, result.telefono);
        Assert.AreEqual(existingHospital.email, result.email);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void UpdateHospital_ShouldThrowKeyNotFoundException_WhenHospitalNotFound()
    {
        // Arrange
        var nonExistentId = 999;
        var hospital = new DatosProfesionales
        {
            nombre = "Test Hospital",
            direccion = "Test Address",
            telefono = "123456789",
            email = "test@hospital.com",
        };

        // Act
        this.hospitalService.UpdateHospital(nonExistentId, hospital);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void UpdateHospital_ShouldThrowArgumentNullException_WhenNullHospitalProvided()
    {
        // Act
        this.hospitalService.UpdateHospital(1, null);
    }

    [TestMethod]
    public void DeleteHospital_ShouldSoftDeleteHospital_WhenHospitalExists()
    {
        // Arrange
        var hospitalId = 1;
        var hospital = new DatosProfesionales
        {
            idDatosPro = hospitalId,
            nombre = "Test Hospital",
            activo = true,
        };

        this.context.DatosProfesionales.Add(hospital);
        this.context.SaveChanges();

        // Act
        var result = this.hospitalService.DeleteHospital(hospitalId);

        // Assert
        Assert.IsTrue(result);
        var deletedHospital = this.context.DatosProfesionales.Find(hospitalId);
        Assert.IsNotNull(deletedHospital);
        Assert.IsFalse(deletedHospital.activo);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void DeleteHospital_ShouldThrowKeyNotFoundException_WhenHospitalNotFound()
    {
        // Act
        this.hospitalService.DeleteHospital(999);
    }

    [TestMethod]
    public void HardDeleteHospital_ShouldRemoveHospital_WhenHospitalExists()
    {
        // Arrange
        var hospitalId = 1;
        var hospital = new DatosProfesionales
        {
            idDatosPro = hospitalId,
            nombre = "Test Hospital",
            activo = true,
        };

        this.context.DatosProfesionales.Add(hospital);
        this.context.SaveChanges();

        // Act
        var result = this.hospitalService.HardDeleteHospital(hospitalId);

        // Assert
        Assert.IsTrue(result);
        var deletedHospital = this.context.DatosProfesionales.Find(hospitalId);
        Assert.IsNull(deletedHospital);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void HardDeleteHospital_ShouldThrowKeyNotFoundException_WhenHospitalNotFound()
    {
        // Act
        this.hospitalService.HardDeleteHospital(999);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void CreateHospital_ShouldThrowDataException_WhenExceptionIsThrown()
    {
        // Arrange
        this.context.Dispose();

        // Act
        this.hospitalService.CreateHospital(new DatosProfesionales());
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void UpdateHospital_ShouldThrowDataException_WhenExceptionIsThrown()
    {
        // Arrange
        this.context.Dispose();

        // Act
        this.hospitalService.UpdateHospital(1, new DatosProfesionales());
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void DeleteHospital_ShouldThrowDataException_WhenExceptionIsThrown()
    {
        // Arrange
        this.context.Dispose();

        // Act
        this.hospitalService.DeleteHospital(1);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void HardDeleteHospital_ShouldThrowDataException_WhenExceptionIsThrown()
    {
        // Arrange
        this.context.Dispose();

        // Act
        this.hospitalService.HardDeleteHospital(1);
    }
}
