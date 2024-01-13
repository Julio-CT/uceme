namespace Uceme.API.Tests.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.API.Controllers;
using Uceme.Library.Services;
using Uceme.Model.Models;

[TestClass]
public class HospitalControllerTests
{
    private HospitalController testClass;
    private Mock<ILogger<HospitalController>> logger;
    private Mock<IHospitalService> hospitalService;

    [TestInitialize]
    public void SetUp()
    {
        this.logger = new Mock<ILogger<HospitalController>>();
        this.hospitalService = new Mock<IHospitalService>();
        this.testClass = new HospitalController(this.logger.Object, this.hospitalService.Object);
    }

    [TestCleanup]
    public void CleanUp()
    {
        this.testClass.Dispose();
    }

    [TestMethod]
    public void CanConstruct()
    {
        // Act
        var instance = new HospitalController(this.logger.Object, this.hospitalService.Object);

        // Assert
        Assert.IsNotNull(instance);
        instance.Dispose();
    }

    [TestMethod]
    public void CannotConstructWithNullLogger()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new HospitalController(default, this.hospitalService.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullHospitalService()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new HospitalController(this.logger.Object, default));
    }

    [TestMethod]
    public void CanCallGetHospitals()
    {
        // Arrangenew
        var hospital = new DatosProfesionales
        {
            idDatosPro = 2081729622,
            nombre = "TestValue1901788151",
            telefono = "TestValue832650482",
            email = "TestValue1819341053",
            direccion = "TestValue445902244",
            texto = "TestValue1158384924",
            foto = "TestValue1640289603",
            activo = true,
        };

        this.hospitalService.Setup(mock => mock.GetHospitals()).Returns(new[]
        {
            hospital,
            new DatosProfesionales
            {
                idDatosPro = 1264674679,
                nombre = "TestValue2003002729",
                telefono = "TestValue1241216817",
                email = "TestValue1260247439",
                direccion = "TestValue1147264480",
                texto = "TestValue557388171",
                foto = "TestValue1150874655",
                activo = true,
            },
            new DatosProfesionales
            {
                idDatosPro = 1421594013,
                nombre = "TestValue1473514872",
                telefono = "TestValue1916474022",
                email = "TestValue2043531315",
                direccion = "TestValue563630733",
                texto = "TestValue243223371",
                foto = "TestValue826527717",
                activo = false,
            },
        });

        // Act
        var result = this.testClass.GetHospitals();

        // Assert
        this.hospitalService.Verify(mock => mock.GetHospitals());
        Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<DatosProfesionales>>));
        Assert.AreEqual(3, result.Value?.ToArray().Length);
        Assert.AreEqual(hospital, result.Value?.ToArray()[0]);
        Assert.AreEqual(1264674679, result.Value?.ToArray()[1].idDatosPro);
        Assert.AreEqual(false, result.Value?.ToArray()[2].activo);
    }

    [TestMethod]
    public void CanCallGetHospital()
    {
        // Arrange
        var hostpitalId = 1878379096;
        var hostpital = new DatosProfesionales
        {
            idDatosPro = hostpitalId,
            nombre = "TestValue968395232",
            telefono = "TestValue1143299189",
            email = "TestValue303385788",
            direccion = "TestValue1666517701",
            texto = "TestValue186598562",
            foto = "TestValue1703356878",
            activo = true,
        };

        this.hospitalService.Setup(mock => mock.GetHospital(It.IsAny<int>())).Returns(hostpital);

        // Act
        var result = this.testClass.GetHospital(hostpitalId);

        // Assert
        this.hospitalService.Verify(mock => mock.GetHospital(It.IsAny<int>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<DatosProfesionales>));
        Assert.AreEqual(hostpital, result.Value);
    }
}
