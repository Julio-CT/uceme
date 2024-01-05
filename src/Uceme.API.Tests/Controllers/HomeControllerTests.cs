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
public class HomeControllerTests
{
    private HomeController testClass;
    private Mock<IMedicoService> medicoService;
    private Mock<IFotosService> fotosService;
    private Mock<ILogger<HomeController>> logger;

    [TestInitialize]
    public void SetUp()
    {
        this.medicoService = new Mock<IMedicoService>();
        this.fotosService = new Mock<IFotosService>();
        this.logger = new Mock<ILogger<HomeController>>();
        this.testClass = new HomeController(this.medicoService.Object, this.fotosService.Object, this.logger.Object);
    }

    [TestMethod]
    public void CanConstruct()
    {
        // Act
        var instance = new HomeController(this.medicoService.Object, this.fotosService.Object, this.logger.Object);

        // Assert
        Assert.IsNotNull(instance);
    }

    [TestMethod]
    public void CannotConstructWithNullMedicoService()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new HomeController(default, this.fotosService.Object, this.logger.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullFotosService()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new HomeController(this.medicoService.Object, default, this.logger.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullLogger()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new HomeController(this.medicoService.Object, this.fotosService.Object, default));
    }

    [TestMethod]
    public void CanCallGetMedicoMinVista()
    {
        // Arrange
        var usuario = new Usuario
        {
            idUsuario = 1567499202,
            nombre = "TestValue356891095",
            apellidos = "TestValue841721633",
            nick = "TestValue1904777048",
            login = "TestValue47658948",
            foto = "TestValue1502219638",
            ultimoupdate = DateTime.UtcNow,
            password = "TestValue263597983",
            newsletter = false,
            linkedin = "TestValue325915856",
            display_order = 1395855734,
            idCurriculum = 1462651888,
            idDatosContacto = 270336494,
            idRol = 510126758,
        };
        this.medicoService.Setup(mock => mock.GetMedicoMinVista(It.IsAny<bool>())).Returns(new[]
        {
            usuario,
            new Usuario
            {
                idUsuario = 996530018,
                nombre = "TestValue353240698",
                apellidos = "TestValue715590640",
                nick = "TestValue287439118",
                login = "TestValue1571103437",
                foto = "TestValue1949742981",
                ultimoupdate = DateTime.UtcNow,
                password = "TestValue1176995254",
                newsletter = true,
                linkedin = "TestValue1888011190",
                display_order = 1681801130,
                idCurriculum = 379142424,
                idDatosContacto = 1615896338,
                idRol = 1032434228,
            },
            new Usuario
            {
                idUsuario = 362002414,
                nombre = "TestValue678701557",
                apellidos = "TestValue1669128548",
                nick = "TestValue845214492",
                login = "TestValue164556710",
                foto = "TestValue1724957921",
                ultimoupdate = DateTime.UtcNow,
                password = "TestValue504893495",
                newsletter = false,
                linkedin = "TestValue512138494",
                display_order = 494431018,
                idCurriculum = 908481453,
                idDatosContacto = 1447265634,
                idRol = 655514323,
            },
        });

        // Act
        var result = this.testClass.GetMedicoMinVista();

        // Assert
        this.medicoService.Verify(mock => mock.GetMedicoMinVista(It.IsAny<bool>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Usuario>>));
        Assert.AreEqual(usuario, result.Value?.ToArray()[0]);
        Assert.AreEqual(996530018, result.Value?.ToArray()[1].idUsuario);
        Assert.AreEqual(1447265634, result.Value?.ToArray()[2].idDatosContacto);
    }

    [TestMethod]
    public void CanCallMostrarFotos()
    {
        // Arrange
        var foto = new Fotos
        {
            idFoto = 120089836,
            nombre = "TestValue849571105",
            texto = "TestValue1947380449",
            destacada = true,
            posicion = 344062816,
        };
        this.fotosService.Setup(mock => mock.GetFotos()).Returns(new[]
        {
            foto,
            new Fotos
            {
                idFoto = 884958447,
                nombre = "TestValue322831170",
                texto = "TestValue173111649",
                destacada = true,
                posicion = 1280403693,
            },
            new Fotos
            {
                idFoto = 1280624059,
                nombre = "TestValue756647506",
                texto = "TestValue811636782",
                destacada = true,
                posicion = 884958447,
            },
        });

        // Act
        var result = this.testClass.MostrarFotos();

        // Assert
        this.fotosService.Verify(mock => mock.GetFotos());
        Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Fotos>>));
        Assert.AreEqual(3, result.Value?.ToArray().Length);
        Assert.AreEqual(foto, result.Value?.ToArray()[0]);
        Assert.AreEqual(884958447, result.Value?.ToArray()[1].idFoto);
        Assert.AreEqual("TestValue811636782", result.Value?.ToArray()[2].texto);
    }
}
