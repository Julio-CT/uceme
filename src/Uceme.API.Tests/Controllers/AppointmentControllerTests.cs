namespace Uceme.API.Tests.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.API.Controllers;
using Uceme.Library.Services;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;

[TestClass]
public class AppointmentControllerTests
{
    private AppointmentController testClass;
    private Mock<ILogger<AppointmentController>> logger;
    private Mock<IAppointmentService> appointmentService;

    [TestInitialize]
    public void SetUp()
    {
        this.logger = new Mock<ILogger<AppointmentController>>();
        this.appointmentService = new Mock<IAppointmentService>();
        this.testClass = new AppointmentController(this.logger.Object, this.appointmentService.Object);
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
        var instance = new AppointmentController(this.logger.Object, this.appointmentService.Object);

        // Assert
        Assert.IsNotNull(instance);
        instance.Dispose();
    }

    [TestMethod]
    public void CannotConstructWithNullLogger()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new AppointmentController(default, this.appointmentService.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullAppointmentService()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new AppointmentController(this.logger.Object, default));
    }

    [TestMethod]
    public void CanCallGetDays()
    {
        // Arrange
        var hospitalId = 156048429;

        this.appointmentService.Setup(mock => mock.GetDays(It.IsAny<int>())).Returns(new[] { 1372003355, 1992714248, 1577160529 });

        // Act
        var result = this.testClass.GetDays(hospitalId);

        // Assert
        this.appointmentService.Verify(mock => mock.GetDays(It.IsAny<int>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<int>>));
        Assert.AreEqual(3, result.Value?.Count());
    }

    [TestMethod]
    public void CanCallGetHours()
    {
        // Arrange
        var appointmentHoursRequest = new AppointmentHoursRequest
        {
            WeekDay = 1962194623,
            HospitalId = "TestValue1095253119",
            Day = 1289192849,
            Month = 327937686,
            Year = 441771161,
        };

        this.appointmentService.Setup(mock => mock.GetHours(It.IsAny<AppointmentHoursRequest>())).Returns(new[] { "TestValue206238028", "TestValue1888943391", "TestValue933704547" });

        // Act
        var result = this.testClass.GetHours(appointmentHoursRequest);

        // Assert
        this.appointmentService.Verify(mock => mock.GetHours(It.IsAny<AppointmentHoursRequest>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<AppointmentHoursResponse>));
        Assert.AreEqual(3, result.Value?.Hours?.Count());
    }

    [TestMethod]
    public void CannotCallGetHoursWithNullAppointmentHoursRequest()
    {
        // Act
        var result = this.testClass.GetHours(default);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<AppointmentHoursResponse>));
        Assert.AreEqual(null, result.Value);
    }

    [TestMethod]
    public async Task CanCallAddAppointmentAsync()
    {
        // Arrange
        var appointmentRequest = new AppointmentRequest
        {
            WeekDay = 166939329,
            HospitalId = 709959810,
            Day = 1523028970,
            Month = 384036050,
            Year = 569147922,
            Hour = "TestValue983290542",
            Name = "TestValue301064087",
            Phone = "TestValue58361252",
            Email = "TestValue75731185",
            ExtraInfo = "TestValue416075738",
        };

        this.appointmentService.Setup(mock => mock.AddAppointmentAsync(It.IsAny<AppointmentRequest>())).ReturnsAsync(false);

        // Act
        var result = await this.testClass.AddAppointmentAsync(appointmentRequest).ConfigureAwait(false);

        // Assert
        this.appointmentService.Verify(mock => mock.AddAppointmentAsync(It.IsAny<AppointmentRequest>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.AreEqual(false, result.Value);

        this.appointmentService.Setup(mock => mock.AddAppointmentAsync(It.IsAny<AppointmentRequest>())).ReturnsAsync(true);

        // Act
        result = await this.testClass.AddAppointmentAsync(appointmentRequest).ConfigureAwait(false);

        // Assert
        this.appointmentService.Verify(mock => mock.AddAppointmentAsync(It.IsAny<AppointmentRequest>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.AreEqual(true, result.Value);
    }

    [TestMethod]
    public async Task CannotCallAddAppointmentAsyncWithNullAppointmentRequest()
    {
        // Act
        var result = await this.testClass.AddAppointmentAsync(default).ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.AreEqual(false, result.Value);
    }

    [TestMethod]
    public void CanCallAppointmentList()
    {
        // Arrange
        this.appointmentService.Setup(mock => mock.GetAppointments()).Returns(new[]
        {
            new Appointment
            {
                idCita = 550021281,
                dia = 1678480265,
                hora = 52827481.08M,
                nombre = "TestValue390663233",
                email = "TestValue1271959662",
                telefono = "TestValue482704706",
                idTurno = 1820801112,
                speciality = "TestValue842273489",
            },
            new Appointment
            {
                idCita = 1744221032,
                dia = 784879203,
                hora = 719201561.76M,
                nombre = "TestValue1165139520",
                email = "TestValue1280649127",
                telefono = "TestValue1326204244",
                idTurno = 1189330136,
                speciality = "TestValue555578589",
            },
            new Appointment
            {
                idCita = 1724644682,
                dia = 717308194,
                hora = 509706472.77M,
                nombre = "TestValue132564320",
                email = "TestValue1383757328",
                telefono = "TestValue1039671035",
                idTurno = 1586428163,
                speciality = "TestValue312226132",
            },
        });

        // Act
        var result = this.testClass.AppointmentList();

        // Assert
        this.appointmentService.Verify(mock => mock.GetAppointments());
        Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Appointment>>));
        Assert.AreEqual(3, result.Value?.Count());
    }

    [TestMethod]
    public void CanCallCloseAppointmentList()
    {
        // Arrange
        this.appointmentService.Setup(mock => mock.GetCloseAppointments()).Returns(new[]
        {
            new Appointment
            {
                idCita = 1822889048,
                dia = 848424917,
                hora = 1764863893.98M,
                nombre = "TestValue794166432",
                email = "TestValue812122799",
                telefono = "TestValue1112468931",
                idTurno = 1097530286,
                speciality = "TestValue824585620",
            },
            new Appointment
            {
                idCita = 1296272170,
                dia = 1576638510,
                hora = 1016867855.52M,
                nombre = "TestValue625051811",
                email = "TestValue152876936",
                telefono = "TestValue1498280158",
                idTurno = 1809228532,
                speciality = "TestValue1479489917",
            },
            new Appointment
            {
                idCita = 1421935654,
                dia = 446919360,
                hora = 723888297.99M,
                nombre = "TestValue1804375605",
                email = "TestValue1712786643",
                telefono = "TestValue1149283456",
                idTurno = 1576794330,
                speciality = "TestValue196824726",
            },
        });

        // Act
        var result = this.testClass.CloseAppointmentList();

        // Assert
        this.appointmentService.Verify(mock => mock.GetCloseAppointments());
        Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Appointment>>));
        Assert.AreEqual(3, result.Value?.Count());
    }

    [TestMethod]
    public void CanCallGetAppointment()
    {
        // Arrange
        var appointmentId = 92052102;

        this.appointmentService.Setup(mock => mock.GetAppointment(It.IsAny<int>())).Returns(new Appointment
        {
            idCita = 1342189561,
            dia = 13569333,
            hora = 1392068211.93M,
            nombre = "TestValue1731239540",
            email = "TestValue1690989876",
            telefono = "TestValue2090650512",
            idTurno = 1179802057,
            speciality = "TestValue1657833893",
        });

        // Act
        var result = this.testClass.GetAppointment(appointmentId);

        // Assert
        this.appointmentService.Verify(mock => mock.GetAppointment(It.IsAny<int>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<Appointment>));
        Assert.AreEqual(1342189561, result.Value?.idCita);
    }

    [TestMethod]
    public void CanCallDeleteAppointment()
    {
        // Arrange
        var appointmentId = 715257832;

        this.appointmentService.Setup(mock => mock.DeleteAppointment(It.IsAny<int>())).Returns(true);

        // Act
        var result = this.testClass.DeleteAppointment(appointmentId);

        // Assert
        this.appointmentService.Verify(mock => mock.DeleteAppointment(It.IsAny<int>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.AreEqual(true, result.Value);
    }

    [TestMethod]
    public void CanCallUpdateAppointment()
    {
        // Arrange
        var appointment = new Cita
        {
            idCita = 1851494264,
            dia = 299064996,
            hora = 558635541.75M,
            nombre = "TestValue402701653",
            email = "TestValue226364524",
            telefono = "TestValue1055563309",
            idTurno = 1182848753,
        };

        this.appointmentService.Setup(mock => mock.UpdateAppointment(It.IsAny<Cita>())).Returns(new Appointment
        {
            idCita = 423534745,
            dia = 847497551,
            hora = 1016368333.2M,
            nombre = "TestValue30150690",
            email = "TestValue631752101",
            telefono = "TestValue1834832564",
            idTurno = 2049364251,
            speciality = "TestValue2016350997",
        });

        // Act
        var result = this.testClass.UpdateAppointment(appointment);

        // Assert
        this.appointmentService.Verify(mock => mock.UpdateAppointment(It.IsAny<Cita>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<Appointment>));
        Assert.AreEqual(423534745, result.Value?.idCita);
    }

    [TestMethod]
    public void CannotCallUpdateAppointmentWithNullAppointment()
    {
        // Act
        var result = this.testClass.UpdateAppointment(default);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<Appointment>));
        Assert.AreEqual(null, result.Value);
    }

    [TestMethod]
    public void CanCallUpdatePastAppointmentsData()
    {
        // Arrange
        this.appointmentService.Setup(mock => mock.UpdatePastAppointmentsData()).Returns(true);

        // Act
        var result = this.testClass.UpdatePastAppointmentsData();

        // Assert
        this.appointmentService.Verify(mock => mock.UpdatePastAppointmentsData());
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.AreEqual(true, result.Value);
    }
}
