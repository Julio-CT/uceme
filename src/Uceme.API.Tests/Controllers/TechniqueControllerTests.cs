namespace Uceme.API.Tests.Controllers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.API.Controllers;
using Uceme.Library.Services;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;
using Uceme.Model.Settings;

[TestClass]
public class TechniqueControllerTests
{
    private TechniqueController testClass;
    private Mock<ITechniqueService> techniqueService;
    private Mock<IOptions<AppSettings>> configuration;
    private Mock<ILogger<TechniqueController>> logger;

    [TestInitialize]
    public void SetUp()
    {
        this.techniqueService = new Mock<ITechniqueService>();
        this.configuration = new Mock<IOptions<AppSettings>>();
        this.logger = new Mock<ILogger<TechniqueController>>();
        this.testClass = new TechniqueController(this.techniqueService.Object, this.configuration.Object, this.logger.Object);
    }

    [TestMethod]
    public void CanConstruct()
    {
        // Act
        var instance = new TechniqueController(this.techniqueService.Object, this.configuration.Object, this.logger.Object);

        // Assert
        Assert.IsNotNull(instance);
    }

    [TestMethod]
    public void CannotConstructWithNullTechniqueService()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new TechniqueController(default, this.configuration.Object, this.logger.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullConfiguration()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new TechniqueController(this.techniqueService.Object, default, this.logger.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullLogger()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new TechniqueController(this.techniqueService.Object, this.configuration.Object, default));
    }

    [TestMethod]
    public void CanCallGetTechniques()
    {
        // Arrange
        this.techniqueService.Setup(mock => mock.GetTechniques()).Returns(new[]
        {
            new Tecnica
            {
                idTecnica = 1806245884,
                titulo = "TestValue629041025",
                fecha = DateTime.UtcNow,
                foto = "TestValue1255328801",
                texto = "TestValue1250751589",
                nombre = "TestValue586016727",
            },
            new Tecnica
            {
                idTecnica = 1542786462,
                titulo = "TestValue1272201115",
                fecha = DateTime.UtcNow,
                foto = "TestValue1234711573",
                texto = "TestValue81124351",
                nombre = "TestValue125531696",
            },
            new Tecnica
            {
                idTecnica = 534868814,
                titulo = "TestValue767359166",
                fecha = DateTime.UtcNow,
                foto = "TestValue1048316901",
                texto = "TestValue1170580293",
                nombre = "TestValue512725058",
            },
        });

        // Act
        var result = this.testClass.GetTechniques();

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Tecnica>>));
        this.techniqueService.Verify(mock => mock.GetTechniques());
    }

    [TestMethod]
    public void CanCallGetTechnique()
    {
        // Arrange
        var techinqueId = 1172865305;

        this.techniqueService.Setup(mock => mock.GetTechnique(It.IsAny<int>())).Returns(new Tecnica
        {
            idTecnica = 859205538,
            titulo = "TestValue720833500",
            fecha = DateTime.UtcNow,
            foto = "TestValue398136342",
            texto = "TestValue621948458",
            nombre = "TestValue2131628067",
        });

        // Act
        var result = this.testClass.GetTechnique(techinqueId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<Tecnica>));
        this.techniqueService.Verify(mock => mock.GetTechnique(It.IsAny<int>()));
    }

    [TestMethod]
    public void CanCallDeleteTech()
    {
        // Arrange
        var techId = 840182971;

        this.techniqueService.Setup(mock => mock.DeleteTechnique(It.IsAny<int>())).Returns(true);

        // Act
        var result = this.testClass.DeleteTech(techId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.IsTrue(result.Value);
        this.techniqueService.Verify(mock => mock.DeleteTechnique(It.IsAny<int>()));
    }

    [TestMethod]
    public void CanCallUpdateTech()
    {
        // Arrange
        var post = new Tecnica
        {
            idTecnica = 1367736128,
            titulo = "TestValue880185007",
            fecha = DateTime.UtcNow,
            foto = "TestValue206722929",
            texto = "TestValue1491397039",
            nombre = "TestValue82382021",
        };

        this.techniqueService.Setup(mock => mock.UpdateTechnique(It.IsAny<Tecnica>())).Returns(new Tecnica
        {
            idTecnica = 1437899086,
            titulo = "TestValue583783504",
            fecha = DateTime.UtcNow,
            foto = "TestValue1629625717",
            texto = "TestValue1187368172",
            nombre = "TestValue44199836",
        });

        // Act
        var result = this.testClass.UpdateTech(post);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<Tecnica>));
        this.techniqueService.Verify(mock => mock.UpdateTechnique(It.IsAny<Tecnica>()));
    }

    [TestMethod]
    public void CannotCallUpdateTechWithNullPost()
    {
        // Act
        var result = this.testClass.UpdateTech(default);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<Tecnica>));
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void CanCallAddTech()
    {
        // Arrange
        var postRequest = new TechniqueRequest
        {
            IdTech = 1741720022,
            Titulo = "TestValue1587777378",
            Fecha = "TestValue649890646",
            Foto = "TestValue1570952770",
            Texto = "TestValue577686591",
            Slug = "TestValue1340384010",
            SeoTitle = "TestValue1949801480",
            MetaDescription = "TestValue1016524631",
        };

        this.techniqueService.Setup(mock => mock.UpdateTechnique(It.IsAny<TechniqueRequest>())).Returns(true);
        this.techniqueService.Setup(mock => mock.AddTechnique(It.IsAny<TechniqueRequest>())).Returns(true);

        // Act
        var result = this.testClass.AddTech(postRequest);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.IsTrue(result.Value);
        this.techniqueService.Verify(mock => mock.UpdateTechnique(It.IsAny<TechniqueRequest>()));
        this.techniqueService.Verify(mock => mock.AddTechnique(It.IsAny<TechniqueRequest>()), Times.Never);
    }

    [TestMethod]
    public void CanCallAddTechNoIdInserts()
    {
        // Arrange
        var postRequest = new TechniqueRequest
        {
            Titulo = "TestValue1587777378",
            Fecha = "TestValue649890646",
            Foto = "TestValue1570952770",
            Texto = "TestValue577686591",
            Slug = "TestValue1340384010",
            SeoTitle = "TestValue1949801480",
            MetaDescription = "TestValue1016524631",
        };

        this.techniqueService.Setup(mock => mock.UpdateTechnique(It.IsAny<TechniqueRequest>())).Returns(true);
        this.techniqueService.Setup(mock => mock.AddTechnique(It.IsAny<TechniqueRequest>())).Returns(true);

        // Act
        var result = this.testClass.AddTech(postRequest);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.IsTrue(result.Value);
        this.techniqueService.Verify(mock => mock.UpdateTechnique(It.IsAny<TechniqueRequest>()), Times.Never);
        this.techniqueService.Verify(mock => mock.AddTechnique(It.IsAny<TechniqueRequest>()));
    }

    [TestMethod]
    public void CanCallAddTechFailsIfInsertFails()
    {
        // Arrange
        var postRequest = new TechniqueRequest
        {
            Titulo = "TestValue1587777378",
            Fecha = "TestValue649890646",
            Foto = "TestValue1570952770",
            Texto = "TestValue577686591",
            Slug = "TestValue1340384010",
            SeoTitle = "TestValue1949801480",
            MetaDescription = "TestValue1016524631",
        };

        this.techniqueService.Setup(mock => mock.UpdateTechnique(It.IsAny<TechniqueRequest>())).Returns(true);
        this.techniqueService.Setup(mock => mock.AddTechnique(It.IsAny<TechniqueRequest>())).Returns(false);

        // Act
        var result = this.testClass.AddTech(postRequest);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.IsFalse(result.Value);
        this.techniqueService.Verify(mock => mock.UpdateTechnique(It.IsAny<TechniqueRequest>()), Times.Never);
        this.techniqueService.Verify(mock => mock.AddTechnique(It.IsAny<TechniqueRequest>()));
    }

    [TestMethod]
    public void CanCallAddTechFailsIfUpdateFails()
    {
        // Arrange
        var postRequest = new TechniqueRequest
        {
            IdTech = 1741720022,
            Titulo = "TestValue1587777378",
            Fecha = "TestValue649890646",
            Foto = "TestValue1570952770",
            Texto = "TestValue577686591",
            Slug = "TestValue1340384010",
            SeoTitle = "TestValue1949801480",
            MetaDescription = "TestValue1016524631",
        };

        this.techniqueService.Setup(mock => mock.UpdateTechnique(It.IsAny<TechniqueRequest>())).Returns(false);
        this.techniqueService.Setup(mock => mock.AddTechnique(It.IsAny<TechniqueRequest>())).Returns(true);

        // Act
        var result = this.testClass.AddTech(postRequest);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.IsFalse(result.Value);
        this.techniqueService.Verify(mock => mock.UpdateTechnique(It.IsAny<TechniqueRequest>()));
        this.techniqueService.Verify(mock => mock.AddTechnique(It.IsAny<TechniqueRequest>()), Times.Never);
    }

    [TestMethod]
    public void CannotCallAddTechWithNullPostRequest()
    {
        // Act
        var result = this.testClass.AddTech(default);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.AreEqual(false, result.Value);
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task CanCallOnTechUploadAsync()
    {
        // Arrange
        var @file = new Mock<IFormFile>().Object;

        this.techniqueService.Setup(mock => mock.GetNextTechImage()).Returns("TestValue254326747");

        // Act
        var result = await this.testClass.OnTechUploadAsync(file).ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<string>));
        this.techniqueService.Verify(mock => mock.GetNextTechImage(), Times.Never);
    }

    [TestMethod]
    public async Task CannotCallOnTechUploadAsyncWithNullFile()
    {
        // Act
        var result = await this.testClass.OnTechUploadAsync(default).ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<string>));
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }
}
