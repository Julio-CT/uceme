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
public class MedicoServiceTests
{
    private Mock<ILogger<MedicoService>> mockLogger;
    private ApplicationDbContext context;
    private MedicoService medicoService;

    [TestInitialize]
    public void Setup()
    {
        this.mockLogger = new Mock<ILogger<MedicoService>>();

        // Create in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        this.context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());

        this.medicoService = new MedicoService(this.mockLogger.Object, this.context);
    }

    [TestMethod]
    public void GetMedicoMinVista_ShouldReturnMedicos_WhenMedicosExist()
    {
        // Arrange
        var usuarios = new List<Usuario>
        {
            new Usuario { idUsuario = 1, nombre = "John", apellidos = "Doe", foto = "photo1.jpg", idRol = 2, display_order = 1 },
            new Usuario { idUsuario = 2, nombre = "Jane", apellidos = "Doe", foto = "photo2.jpg", idRol = 2, display_order = 2 },
            new Usuario { idUsuario = 3, nombre = "Mark", apellidos = "Twain", foto = "photo3.jpg", idRol = 1, display_order = 3 },
        };

        this.context.Usuario.AddRange(usuarios);
        this.context.SaveChanges();

        // Act
        var result = this.medicoService.GetMedicoMinVista(false);

        // Assert
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void GetMedicoMinVista_ShouldThrowDataException_WhenExceptionIsThrown()
    {
        // Arrange - use a disposed context to force an exception
        this.context.Dispose();

        // Act
        this.medicoService.GetMedicoMinVista(false);
    }

    [TestCleanup]
    public void Cleanup()
    {
        this.context.Dispose();
    }
}
