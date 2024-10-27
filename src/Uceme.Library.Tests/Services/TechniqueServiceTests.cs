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
public class TechniqueServiceTests
{
    private Mock<ILogger<TechniqueService>> mockLogger;
    private ApplicationDbContext context;
    private TechniqueService techniqueService;

    [TestInitialize]
    public void Setup()
    {
        this.mockLogger = new Mock<ILogger<TechniqueService>>();

        // Create in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        this.context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());

        this.techniqueService = new TechniqueService(this.mockLogger.Object, this.context);
    }

    [TestMethod]
    public void GetTechniques_ShouldReturnTechniques_WhenTechniquesExist()
    {
        // Arrange
        var techniques = new List<Tecnica>
        {
            new Tecnica { idTecnica = 1, nombre = "Technique 1" },
            new Tecnica { idTecnica = 2, nombre = "Technique 2" },
        };

        this.context.Tecnica.AddRange(techniques);
        this.context.SaveChanges();

        // Act
        var result = this.techniqueService.GetTechniques();

        // Assert
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetTechnique_ShouldReturnTechnique_WhenTechniqueExists()
    {
        // Arrange
        var techniqueId = 1;
        var technique = new Tecnica { idTecnica = techniqueId, nombre = "Technique 1" };

        this.context.Tecnica.Add(technique);
        this.context.SaveChanges();

        // Act
        var result = this.techniqueService.GetTechnique(techniqueId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(techniqueId, result.idTecnica);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void GetTechnique_ShouldThrowDataException_WhenExceptionIsThrown()
    {
        // Arrange - use a disposed context to force an exception
        var techniqueId = 1;
        this.context.Dispose();

        // Act
        this.techniqueService.GetTechnique(techniqueId);
    }

    [TestCleanup]
    public void Cleanup()
    {
        this.context.Dispose();
    }
}
