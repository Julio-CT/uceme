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

namespace Uceme.Tests.Services;

[TestClass]
public class GptFotosServiceTests
{
    private Mock<ILogger<FotosService>> mockLogger;
    private FotosService fotosService;
    private ApplicationDbContext context;

    [TestInitialize]
    public void Setup()
    {
        this.mockLogger = new Mock<ILogger<FotosService>>();

        // Create in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        this.context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());

        this.fotosService = new FotosService(this.mockLogger.Object, this.context);
    }

    [TestMethod]
    public void GetFotos_ShouldReturnFotos_WhenFotosExist()
    {
        // Arrange
        var fotos = new List<Foto>
        {
            new Foto { destacada = true },
            new Foto { destacada = true },
            new Foto { destacada = false },
        };

        this.context.Fotos.AddRange(fotos);
        this.context.SaveChanges();

        // Act
        var result = this.fotosService.GetFotos();

        // Assert
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void GetFotos_ShouldThrowDataException_WhenExceptionIsThrown()
    {
        // Arrange - use a disposed context to force an exception
        this.context.Dispose();

        // Act
        this.fotosService.GetFotos();
    }

    [TestCleanup]
    public void Cleanup()
    {
        this.context.Dispose();
    }
}
