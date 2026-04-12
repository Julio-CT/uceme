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
using Uceme.Model.DataContracts;
using Uceme.Model.Models;

namespace Uceme.Library.Tests.Services;

[TestClass]
public class TurnoServiceTests
{
    private Mock<ILogger<TurnoService>> mockLogger;
    private ApplicationDbContext context;
    private TurnoService turnoService;

    [TestInitialize]
    public void Setup()
    {
        this.mockLogger = new Mock<ILogger<TurnoService>>();

        // Create in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        this.context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());

        this.turnoService = new TurnoService(this.mockLogger.Object, this.context);
    }

    [TestMethod]
    public void GetTurnos_ShouldReturnTurnos_WhenTurnosExist()
    {
        // Arrange
        var turnos = new List<Turno>
        {
            new Turno { dia = 1, inicio = 9.00m, fin = 13.00m, paralelas = 2, porhora = 1, idHospital = 1 },
            new Turno { dia = 2, inicio = 14.00m, fin = 18.00m, paralelas = 2, porhora = 1, idHospital = 1 },
        };

        this.context.Turno.AddRange(turnos);
        this.context.SaveChanges();

        // Act
        var result = this.turnoService.GetTurnos();

        // Assert
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void GetTurnos_ShouldThrowDataException_WhenExceptionIsThrown()
    {
        // Arrange
        this.context.Dispose();

        // Act
        this.turnoService.GetTurnos();
    }

    [TestMethod]
    public void GetTurns_WhenNoTurns_ReturnsEmptyList()
    {
        // Act
        var result = this.turnoService.GetTurnos();

        // Assert
        Assert.AreEqual(result.Count(), 0);
    }

    [TestMethod]
    public void GetTurno_ShouldReturnTurno_WhenTurnoExists()
    {
        // Arrange
        var turnoId = 1;
        var turno = new Turno
        {
            idTurno = turnoId,
            dia = 1,
            inicio = 9.00m,
            fin = 13.00m,
            paralelas = 2,
            porhora = 1,
            idHospital = 1,
        };

        this.context.Turno.Add(turno);
        this.context.SaveChanges();

        // Act
        var result = this.turnoService.GetTurno(turnoId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(turnoId, result.IdTurno);
        Assert.AreEqual(turno.dia, result.Dia);
        Assert.AreEqual(Foundation.Utilities.DateTimeUtils.TimeToString(turno.inicio), result.Inicio);
        Assert.AreEqual(Foundation.Utilities.DateTimeUtils.TimeToString(turno.fin), result.Fin);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void GetTurno_ShouldThrowDataException_WhenTurnoNotFound()
    {
        // Act
        this.turnoService.GetTurno(999);
    }

    [TestMethod]
    public void CreateTurno_ShouldCreateTurno_WhenValidDataProvided()
    {
        // Arrange
        var turn = new Turn
        {
            Dia = 1,
            Inicio = "09:00",
            Fin = "13:00",
            Paralelas = 2,
            Porhora = 1,
            IdHospital = 1,
        };

        // Act
        var result = this.turnoService.CreateTurno(turn);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(turn.Dia, result.Dia);
        Assert.AreEqual(turn.Inicio, result.Inicio);
        Assert.AreEqual(turn.Fin, result.Fin);
        Assert.AreEqual(turn.Paralelas, result.Paralelas);
        Assert.AreEqual(turn.Porhora, result.Porhora);
        Assert.AreEqual(turn.IdHospital, result.IdHospital);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void CreateTurno_ShouldThrowArgumentNullException_WhenNullTurnoProvided()
    {
        // Act
        this.turnoService.CreateTurno(null);
    }

    [TestMethod]
    public void UpdateTurno_ShouldUpdateTurno_WhenValidDataProvided()
    {
        // Arrange
        var turnoId = 1;
        var existingTurno = new Turno
        {
            idTurno = turnoId,
            dia = 1,
            inicio = 9.00m,
            fin = 13.00m,
            paralelas = 2,
            porhora = 1,
            idHospital = 1,
        };

        this.context.Turno.Add(existingTurno);
        this.context.SaveChanges();

        var updatedTurn = new Turn
        {
            Dia = 2,
            Inicio = "14:00",
            Fin = "18:00",
            Paralelas = 3,
            Porhora = 2,
            IdHospital = 1,
        };

        // Act
        var result = this.turnoService.UpdateTurno(turnoId, updatedTurn);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(turnoId, result.IdTurno);
        Assert.AreEqual(updatedTurn.Dia, result.Dia);
        Assert.AreEqual(updatedTurn.Inicio, result.Inicio);
        Assert.AreEqual(updatedTurn.Fin, result.Fin);
        Assert.AreEqual(updatedTurn.Paralelas, result.Paralelas);
        Assert.AreEqual(updatedTurn.Porhora, result.Porhora);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void UpdateTurno_ShouldThrowKeyNotFoundException_WhenTurnoNotFound()
    {
        // Arrange
        var nonExistentId = 999;
        var turn = new Turn
        {
            Dia = 1,
            Inicio = "9.00",
            Fin = "13.00",
            Paralelas = 2,
            Porhora = 1,
            IdHospital = 1,
        };

        // Act
        this.turnoService.UpdateTurno(nonExistentId, turn);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void UpdateTurno_ShouldThrowArgumentNullException_WhenNullTurnoProvided()
    {
        // Act
        this.turnoService.UpdateTurno(1, null);
    }

    [TestMethod]
    public void DeleteTurno_ShouldRemoveTurno_WhenTurnoExists()
    {
        // Arrange
        var turnoId = 1;
        var turno = new Turno
        {
            idTurno = turnoId,
            dia = 1,
            inicio = 9.00m,
            fin = 13.00m,
            paralelas = 2,
            porhora = 1,
            idHospital = 1,
        };

        this.context.Turno.Add(turno);
        this.context.SaveChanges();

        // Act
        var result = this.turnoService.DeleteTurno(turnoId);

        // Assert
        Assert.IsTrue(result);
        var deletedTurno = this.context.Turno.Find(turnoId);
        Assert.IsNull(deletedTurno);
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void DeleteTurno_ShouldThrowKeyNotFoundException_WhenTurnoNotFound()
    {
        // Act
        this.turnoService.DeleteTurno(999);
    }

    [TestMethod]
    public void GetTurnosByHospital_ShouldReturnTurnos_WhenTurnosExistForHospital()
    {
        // Arrange
        var hospitalId = 1;
        var turnos = new List<Turno>
        {
            new Turno { dia = 1, inicio = 9.00m, fin = 13.00m, paralelas = 2, porhora = 1, idHospital = hospitalId },
            new Turno { dia = 2, inicio = 14.00m, fin = 18.00m, paralelas = 2, porhora = 1, idHospital = hospitalId },
            new Turno { dia = 1, inicio = 9.00m, fin = 13.00m, paralelas = 2, porhora = 1, idHospital = 2 },
        };

        this.context.Turno.AddRange(turnos);
        this.context.SaveChanges();

        // Act
        var result = this.turnoService.GetTurnosByHospital(hospitalId);

        // Assert
        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.All(t => t.IdHospital == hospitalId));
    }

    [TestMethod]
    [ExpectedException(typeof(DataException))]
    public void GetTurnosByHospital_ShouldThrowDataException_WhenExceptionIsThrown()
    {
        // Arrange
        this.context.Dispose();

        // Act
        this.turnoService.GetTurnosByHospital(1);
    }
}
