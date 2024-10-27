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
public class ScheduleServiceTests
{
    private Mock<ILogger<ScheduleService>> mockLogger;
    private ApplicationDbContext context;
    private ScheduleService scheduleService;

    [TestInitialize]
    public void Setup()
    {
        this.mockLogger = new Mock<ILogger<ScheduleService>>();

        // Create in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        this.context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());

        this.scheduleService = new ScheduleService(this.mockLogger.Object, this.context);
    }

    [TestMethod]
    public void GetTurns_ShouldReturnTurns_WhenTurnsExist()
    {
        // Arrange
        var turns = new List<Turno>
        {
            new Turno { idTurno = 1, dia = 1 },
            new Turno { idTurno = 2, dia = 2 },
        };

        this.context.Turno.AddRange(turns);
        this.context.SaveChanges();

        // Act
        var result = this.scheduleService.GetTurns();

        // Assert
        Assert.AreEqual(2, result.Count());

        // Verify the order (descending by idTurno then by dia)
        Assert.AreEqual(2, result.First().idTurno);
        Assert.AreEqual(1, result.Last().idTurno);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void GetTurns_ShouldThrowDataException_WhenExceptionIsThrown()
    {
        // Arrange - use a disposed context to force an exception
        this.context.Dispose();

        // Act
        this.scheduleService.GetTurns();
    }

    [TestCleanup]
    public void Cleanup()
    {
        this.context.Dispose();
    }
}
