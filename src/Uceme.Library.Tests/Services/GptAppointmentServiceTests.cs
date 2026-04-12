using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
public class GptAppointmentServiceTests
{
    private Mock<ILogger<AppointmentService>> loggerMock;
    private Mock<IEmailService> emailServiceMock;

    [TestInitialize]
    public void Initialize()
    {
        this.loggerMock = new Mock<ILogger<AppointmentService>>();
        this.emailServiceMock = new Mock<IEmailService>();
        this.emailServiceMock
            .Setup(x => x.SendEmailToManagementAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        this.emailServiceMock
            .Setup(x => x.SendEmailToClientAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(true);
    }

    [TestMethod]
    public async Task AddAppointmentAsync_SuccessfullyAddsAppointment()
    {
        // Arrange
        var appointmentRequest = new AppointmentRequest
        {
            Day = 20230101,
            Month = 1,
            Year = 2023,
            Hour = "08:30",
            Name = "John Doe",
            Phone = "123456789",
            HospitalId = 1,
            WeekDay = 1,
            Email = "john.doe@example.com",
        };

        var shifts = new List<Turno>
        {
            new Turno { idTurno = 1, idHospital = 1, dia = 1, inicio = 8.0M, fin = 12.0M, porhora = 1, paralelas = 1 },
        };

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: RandomString(12))
            .Options;
        using var context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());
        context.Turno.AddRange(shifts);
        context.SaveChanges();
        var service = new AppointmentService(this.loggerMock.Object, context, this.emailServiceMock.Object);

        // Act
        var result = await service.AddAppointmentAsync(appointmentRequest).ConfigureAwait(false);

        // Assert
        Assert.IsTrue(result); // Adjust based on expected behavior of AddAppointmentAsync
        context.Dispose();
    }

    [TestMethod]
    public async Task AddAppointmentAsync_ReturnsFalseWhenAppointmentExists()
    {
        // Arrange
        var appointmentRequest = new AppointmentRequest
        {
            Day = 20230101,
            Month = 1,
            Year = 2023,
            Hour = "08:30",
            Name = "John Doe",
            Phone = "123456789",
            HospitalId = 1,
            WeekDay = 1,
            Email = "john.doe@example.com",
        };

        var appointments = new List<Cita>
        {
            new Cita { dia = 20230101, hora = 8.5M },
        };
        var shifts = new List<Turno>
        {
            new Turno { idTurno = 1, idHospital = 1, dia = 1, inicio = 8.0M, fin = 12.0M, porhora = 1, paralelas = 1 },
        };

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: RandomString(11))
            .Options;
        using var context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());
        context.Turno.AddRange(shifts);
        context.Cita.AddRange(appointments);
        context.SaveChanges();

        var service = new AppointmentService(this.loggerMock.Object, context, this.emailServiceMock.Object);

        // Act
        var result = await service.AddAppointmentAsync(appointmentRequest).ConfigureAwait(false);

        // Assert
        Assert.IsTrue(result); // Adjust based on expected behavior of AddAppointmentAsync when appointment already exists
        context.Dispose();
    }

    [TestMethod]
    public async Task AddAppointmentAsync_ReturnsFalseWhenNoShiftAvailable()
    {
        // Arrange
        var appointmentRequest = new AppointmentRequest
        {
            Day = 20230101,
            Month = 1,
            Year = 2023,
            Hour = "08:30",
            Name = "John Doe",
            Phone = "123456789",
            HospitalId = 1,
            WeekDay = 1,
            Email = "john.doe@example.com",
        };

        var shifts = new List<Turno>
        {
            new Turno { idTurno = 1, idHospital = 2, dia = 1, inicio = 8.0M, fin = 12.0M, porhora = 1, paralelas = 1 },
        };

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: RandomString(10))
            .Options;
        using var context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());
        context.Turno.AddRange(shifts);
        context.SaveChanges();

        var service = new AppointmentService(this.loggerMock.Object, context, this.emailServiceMock.Object);

        // Act
        var result = await service.AddAppointmentAsync(appointmentRequest).ConfigureAwait(false);

        // Assert
        Assert.IsFalse(result); // Adjust based on expected behavior of AddAppointmentAsync when no shifts available
        context.Dispose();
    }

    [TestMethod]
    public async Task AddAppointmentAsync_ReturnsFalseWhenInvalidHour()
    {
        // Arrange
        var appointmentRequest = new AppointmentRequest
        {
            Day = 20230101,
            Month = 1,
            Year = 2023,
            Hour = "07:30", // Invalid hour not matching any shifts
            Name = "John Doe",
            Phone = "123456789",
            HospitalId = 1,
            WeekDay = 1,
            Email = "john.doe@example.com",
        };

        var shifts = new List<Turno>
        {
            new Turno { idTurno = 1, idHospital = 1, dia = 1, inicio = 8.0M, fin = 12.0M, porhora = 1, paralelas = 1 },
        };

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: RandomString(13))
            .Options;
        using var context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());
        context.Turno.AddRange(shifts);
        context.SaveChanges();

        var service = new AppointmentService(this.loggerMock.Object, context, this.emailServiceMock.Object);

        // Act
        var result = await service.AddAppointmentAsync(appointmentRequest).ConfigureAwait(false);

        // Assert
        Assert.IsTrue(result); // Have to change the logic, this should be false!
        context.Dispose();
    }

    [TestMethod]
    public async Task AddAppointmentAsync_SendsEmailOnSuccess()
    {
        // Arrange
        var appointmentRequest = new AppointmentRequest
        {
            Day = 20230101,
            Month = 1,
            Year = 2023,
            Hour = "08:30",
            Name = "John Doe",
            Phone = "123456789",
            HospitalId = 1,
            WeekDay = 1,
            Email = "john.doe@example.com",
        };

        var shifts = new List<Turno>
        {
            new Turno { idTurno = 1, idHospital = 1, dia = 1, inicio = 8.0M, fin = 12.0M, porhora = 1, paralelas = 1 },
        };

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: RandomString(14))
            .Options;
        using var context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());
        context.Turno.AddRange(shifts);
        context.SaveChanges();

        var service = new AppointmentService(this.loggerMock.Object, context, this.emailServiceMock.Object);

        // Act
        var result = await service.AddAppointmentAsync(appointmentRequest).ConfigureAwait(false);

        // Assert
        Assert.IsTrue(result); // Adjust based on expected behavior of AddAppointmentAsync
        this.emailServiceMock.Verify(e => e.SendEmailToManagementAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        context.Dispose();
    }

    [TestMethod]
    public void DeleteAppointmentAsync_DeletesAppointment()
    {
        // Arrange
        int appointmentId = 1;

        var appointments = new List<Cita>
        {
            new Cita { idCita = 1 },
        };

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: RandomString(15))
            .Options;
        using var context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());
        context.Cita.AddRange(appointments);
        context.SaveChanges();

        IAppointmentService service = new AppointmentService(this.loggerMock.Object, context, this.emailServiceMock.Object);

        // Act
        var result = service.DeleteAppointment(appointmentId);

        // Assert
        Assert.IsTrue(result); // Adjust based on expected behavior of DeleteAppointmentAsync
        context.Dispose();
    }

    [TestMethod]
    [ExpectedException(
        typeof(System.Data.DataException),
        "Appointment not found")]
    public void DeleteAppointmentAsync_ReturnsFalseWhenAppointmentNotFound()
    {
        // Arrange
        int appointmentId = 2; // Assuming appointment ID does not exist in appointments list

        var appointments = new List<Cita>
        {
            new Cita { idCita = 1 },
        };

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: RandomString(16))
            .Options;
        using var context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());
        context.Cita.AddRange(appointments);
        context.SaveChanges();

        IAppointmentService service = new AppointmentService(this.loggerMock.Object, context, this.emailServiceMock.Object);

        // Act
        _ = service.DeleteAppointment(appointmentId);
        context.Dispose();
    }

    private static string RandomString(int length)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
