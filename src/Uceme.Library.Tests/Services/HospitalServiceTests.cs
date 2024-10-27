namespace Uceme.Library.Tests.Services;

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

    // [TestMethod]
    // [ExpectedException(typeof(DataException))]
    // public void GetHospital_ShouldThrowDataException_WhenExceptionIsThrown()
    // {
    //     // Arrange
    //     var hospitalId = 1;
    //     this.mockContext.Setup(c => c.DatosProfesionales).Throws(new Exception("Test exception"));
    //     // Act
    //     this.hospitalService.GetHospital(hospitalId);
    // }
}
