namespace Uceme.Library.Tests.Services;

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMoqCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.Library.Services;
using Uceme.Model.Data;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;

[TestClass]
public class AppointmentServiceTests
{
    private AppointmentService testClass;
    private Mock<IEmailService> emailServiceMock;
    private Mock<ILogger<AppointmentService>> logger;
    private ApplicationDbContext context;

    [TestInitialize]
    public void SetUp()
    {
        this.logger = new Mock<ILogger<AppointmentService>>();
        this.emailServiceMock = new Mock<IEmailService>();
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: RandomString(12))
            .Options;
        this.context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());

        this.MockDataInContext();

        this.testClass = new AppointmentService(this.logger.Object, this.context, this.emailServiceMock.Object);
    }

    [TestCleanup]
    public void CleanUp()
    {
        this.context.Dispose();
    }

    [TestMethod]
    public void CanCallGetAppointments()
    {
        //// ARRANGE
        AutoMoqer mocker = new AutoMoqer();
        mocker.SetInstance<IApplicationDbContext>(this.context);

        AppointmentService sut = mocker.Create<AppointmentService>();

        //// ACT
        IEnumerable<Appointment>? appointments = sut.GetAppointments();

        //// ASSERT
        Assert.IsNotNull(appointments);
        Assert.AreEqual(2, appointments.Count());
    }

    [TestMethod]
    public void CanCallGetAppointmentsEvents()
    {
        //// ARRANGE
        AutoMoqer mocker = new AutoMoqer();
        mocker.SetInstance<IApplicationDbContext>(this.context);

        AppointmentService sut = mocker.Create<AppointmentService>();

        //// ACT
        IEnumerable<CalendarEvent>? appointments = sut.GetAppointmentsEvents();

        //// ASSERT
        Assert.IsNotNull(appointments);
        IEnumerable<CalendarEvent> calendarEvents = appointments as CalendarEvent[] ?? appointments.ToArray();
        Assert.AreEqual(2, calendarEvents.Count());
        Assert.AreEqual("2024-01-18-1-25", calendarEvents.ToArray()[0].end);
        Assert.AreEqual("2024-01-18-1-6", calendarEvents.ToArray()[0].start);
        Assert.AreEqual("somehospitalname: as.", calendarEvents.ToArray()[1].title);
        Assert.AreEqual("Telf: 123, Email: asd@as", calendarEvents.ToArray()[1].description);
    }

    [TestMethod]
    public void CanConstruct()
    {
        // Act
        AppointmentService instance = new AppointmentService(this.logger.Object, this.context, this.emailServiceMock.Object);

        // Assert
        Assert.IsNotNull(instance);
    }

    [TestMethod]
    public void CannotConstructWithNullLogger()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new AppointmentService(default, this.context, this.emailServiceMock.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullContext()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new AppointmentService(this.logger.Object, default, this.emailServiceMock.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullEmailService()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new AppointmentService(this.logger.Object, this.context, default));
    }

    [TestMethod]
    public void CanCallGetCloseAppointments()
    {
        //// ARRANGE
        int tomorrowsDate = GetIntDate(1);

        Cita appointment = new Cita
        {
            dia = tomorrowsDate,
            email = "asd@as",
            hora = 1.1M,
            idCita = 2146510150,
            idTurno = 1,
            nombre = "as",
            telefono = "123",
        };
        this.context.Cita.Add(appointment);

        this.context.SaveChanges();
        AutoMoqer mocker = new AutoMoqer();
        mocker.SetInstance<IApplicationDbContext>(this.context);

        AppointmentService sut = mocker.Create<AppointmentService>();

        // Act
        IEnumerable<Appointment> result = sut.GetCloseAppointments();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void CanCallGetHours()
    {
        // Arrange
        Cita appointment = new Cita
        {
            idCita = 750079141,
            dia = 62888007,
            hora = 1.00M,
            nombre = "TestValue1412440326",
            email = "TestValue493815820",
            telefono = "TestValue116430283",
            idTurno = 1246174872,
        };

        this.context.Cita.Add(appointment);

        this.context.Turno.Add(new Turno
        {
            idTurno = 1246174872,
            idHospital = 1,
            dia = 1356295792,
            inicio = 1,
            fin = 2,
            paralelas = 2,
            porhora = 3,
        });

        this.context.SaveChanges();

        AppointmentHoursRequest appointmentHoursRequest = new AppointmentHoursRequest
        {
            WeekDay = 1356295792,
            HospitalId = "1",
            Day = 953543215,
            Month = 1649712905,
            Year = 1550217875,
        };

        // Act
        IEnumerable<string> result = this.testClass.GetHours(appointmentHoursRequest);

        // Assert
        Assert.IsInstanceOfType(result, typeof(IEnumerable<string>));
        Assert.AreEqual(4, result.Count());
        Assert.AreEqual("01:40", result.ToArray()[2]);
        Assert.AreEqual("02:00", result.ToArray()[3]);
    }

    [TestMethod]
    public void CanCallGetHoursOneHourBusy()
    {
        // Arrange
        Cita appointment = new Cita
        {
            idCita = 750079141,
            dia = 62888007,
            hora = 1.00M,
            nombre = "TestValue1412440326",
            email = "TestValue493815820",
            telefono = "TestValue116430283",
            idTurno = 1246174872,
        };

        Cita appointment2 = new Cita
        {
            idCita = 750079142,
            dia = 62888007,
            hora = 1.00M,
            nombre = "TestValue1412440326",
            email = "TestValue493815820",
            telefono = "TestValue116430283",
            idTurno = 1246174872,
        };

        this.context.Cita.Add(appointment);
        this.context.Cita.Add(appointment2);

        this.context.Turno.Add(new Turno
        {
            idTurno = 1246174872,
            idHospital = 1,
            dia = 1356295792,
            inicio = 1,
            fin = 2,
            paralelas = 2,
            porhora = 3,
        });

        this.context.SaveChanges();

        AppointmentHoursRequest appointmentHoursRequest = new AppointmentHoursRequest
        {
            WeekDay = 1356295792,
            HospitalId = "1",
            Day = 953543215,
            Month = 1649712905,
            Year = 1550217875,
        };

        // Act
        IEnumerable<string> result = this.testClass.GetHours(appointmentHoursRequest);

        // Assert
        Assert.IsInstanceOfType(result, typeof(IEnumerable<string>));
        Assert.AreEqual(3, result.Count());
        Assert.AreEqual("01:40", result.ToArray()[1]);
        Assert.AreEqual("02:00", result.ToArray()[2]);
    }

    [TestMethod]
    public void CannotCallGetHoursWithNullAppointmentHoursRequest()
    {
        Assert.ThrowsException<ArgumentNullException>(() => this.testClass.GetHours(default));
    }

    [TestMethod]
    public void CanCallGetDays()
    {
        // Arrange
        int hospitalId = 2085461919;
        this.context.Turno.Add(new Turno
        {
            idTurno = 1246174872,
            idHospital = hospitalId,
            dia = 1356295792,
            inicio = 1,
            fin = 2,
            paralelas = 2,
            porhora = 3,
        });

        this.context.SaveChanges();

        // Act
        IEnumerable<int> result = this.testClass.GetDays(hospitalId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(IEnumerable<int>));
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual(1356295792, result.ToArray()[0]);
    }

    [TestMethod]
    public async Task CanCallAddAppointmentAsync()
    {
        // Arrange
        AppointmentRequest appointmentRequest = new AppointmentRequest
        {
            WeekDay = 1356295791,
            HospitalId = 1,
            Day = 12,
            Month = 12,
            Year = 2034,
            Hour = "1200",
            Name = "TestValue1013016022",
            Phone = "TestValue1752999496",
            Email = "TestValue1241331825",
            ExtraInfo = "TestValue675678694",
        };

        this.emailServiceMock.Setup(mock => mock.SendEmailToManagementAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

        // Act
        bool result = await this.testClass.AddAppointmentAsync(appointmentRequest).ConfigureAwait(false);

        // Assert
        this.emailServiceMock.Verify(mock => mock.SendEmailToManagementAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

        Assert.IsTrue(result);
        Assert.AreEqual(1, this.context.Cita.Where(x => x.email == appointmentRequest.Email && x.telefono == appointmentRequest.Phone && x.nombre == appointmentRequest.Name && x.hora == 1 && x.idTurno == 2 && x.dia == 20341212).Count());
    }

    [TestMethod]
    public async Task CannotCallAddAppointmentAsyncWithNullAppointmentRequest()
    {
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => this.testClass.AddAppointmentAsync(default)).ConfigureAwait(false);
    }

    [TestMethod]
    public void CanCallUpdatePastAppointmentsData()
    {
        //// ARRANGE
        int tomorrowsDate = GetIntDate(AppointmentService.DataExpiryDays - 1);

        Cita appointment = new Cita
        {
            dia = tomorrowsDate,
            email = "asd@as",
            hora = 1.1M,
            idCita = 2146510150,
            idTurno = 1,
            nombre = "as",
            telefono = "123",
        };
        this.context.Cita.Add(appointment);

        this.context.SaveChanges();

        // Act
        bool result = this.testClass.UpdatePastAppointmentsData();

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual("eliminado", this.context.Cita.First(x => x.idCita == 2146510150).email);
        Assert.AreEqual("eliminado", this.context.Cita.First(x => x.idCita == 2146510150).telefono);
    }

    [TestMethod]
    public void CanCallGetAppointment()
    {
        // Arrange
        Cita appointment = new Cita
        {
            dia = 1,
            email = "asd@as",
            hora = 1.1M,
            idCita = 2146510150,
            idTurno = 1,
            nombre = "as",
            telefono = "123",
        };
        this.context.Cita.Add(appointment);

        this.context.SaveChanges();

        // Act
        Appointment result = ((IAppointmentService)this.testClass).GetAppointment(appointment.idCita);

        // Assert
        Assert.IsInstanceOfType(result, typeof(Appointment));
        Assert.AreEqual(appointment.idCita, result.idCita);
        Assert.AreEqual(appointment.dia, result.dia);
        Assert.AreEqual(appointment.hora, result.hora);
        Assert.AreSame(appointment.nombre, result.nombre);
        Assert.AreSame(appointment.email, result.email);
        Assert.AreSame(appointment.telefono, result.telefono);
        Assert.AreEqual(appointment.idTurno, result.idTurno);
    }

    [TestMethod]
    public void CanCallDeleteAppointment()
    {
        // Arrange
        int appointmentId = 1943569314;
        this.context.Cita.Add(new Cita
        {
            dia = 1,
            email = "asd@as",
            hora = 1.1M,
            idCita = 1943569314,
            idTurno = 1,
            nombre = "as",
            telefono = "123",
        });

        this.context.SaveChanges();

        // Act
        bool result = ((IAppointmentService)this.testClass).DeleteAppointment(appointmentId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CanCallUpdateAppointment()
    {
        // Arrange
        Cita appointment = new Cita
        {
            idCita = 750079141,
            dia = 1356295792,
            hora = 1167195939.03M,
            nombre = "TestValue1412440326",
            email = "TestValue493815820",
            telefono = "TestValue116430283",
            idTurno = 1246174872,
        };

        this.context.Cita.Add(new Cita
        {
            dia = 1,
            email = "asd@as",
            hora = 1.1M,
            idCita = 750079141,
            idTurno = 1,
            nombre = "as",
            telefono = "123",
        });

        this.context.Turno.Add(new Turno
        {
            idTurno = 1246174872,
            idHospital = 1,
        });

        this.context.SaveChanges();

        // Act
        Appointment result = ((IAppointmentService)this.testClass).UpdateAppointment(appointment);

        // Assert
        Assert.IsInstanceOfType(result, typeof(Appointment));
        Assert.AreEqual(appointment.idCita, result.idCita);
        Assert.AreEqual(appointment.dia, result.dia);
        Assert.AreEqual(appointment.hora, result.hora);
        Assert.AreSame(appointment.nombre, result.nombre);
        Assert.AreSame(appointment.email, result.email);
        Assert.AreSame(appointment.telefono, result.telefono);
        Assert.AreEqual(appointment.idTurno, result.idTurno);
    }

    [TestMethod]
    public void CannotCallUpdateAppointmentWithNullAppointment()
    {
        Assert.ThrowsException<DataException>(() => ((IAppointmentService)this.testClass).UpdateAppointment(default));
    }

    [TestMethod]
    public void UpdateAppointmentPerformsMapping()
    {
        // Arrange
        Cita appointment = new Cita
        {
            idCita = 1099130085,
            dia = 452068112,
            hora = 382878360.81M,
            nombre = "TestValue1502509491",
            email = "TestValue2118718613",
            telefono = "TestValue988009541",
            idTurno = 1860430658,
        };

        this.context.Cita.Add(new Cita
        {
            dia = 1,
            email = "asd@as",
            hora = 1.1M,
            idCita = 1099130085,
            idTurno = 1,
            nombre = "as",
            telefono = "123",
        });

        this.context.Turno.Add(new Turno
        {
            idTurno = 1860430658,
            idHospital = 1,
        });

        this.context.SaveChanges();

        // Act
        Appointment result = ((IAppointmentService)this.testClass).UpdateAppointment(appointment);

        // Assert
        Assert.AreEqual(appointment.idCita, result.idCita);
        Assert.AreEqual(appointment.dia, result.dia);
        Assert.AreEqual(appointment.hora, result.hora);
        Assert.AreSame(appointment.nombre, result.nombre);
        Assert.AreSame(appointment.email, result.email);
        Assert.AreSame(appointment.telefono, result.telefono);
        Assert.AreEqual(appointment.idTurno, result.idTurno);
    }

    private static string RandomString(int length)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static int GetIntDate(int delta)
    {
        string tomorrowsYear = DateTime.Now.AddDays(delta).Year.ToString(CultureInfo.CurrentCulture);
        string tomorrowsMonth = DateTime.Now.AddDays(delta).Month.ToString("00", CultureInfo.CurrentCulture);
        string tomorrowsDay = DateTime.Now.AddDays(delta).Day.ToString("00", CultureInfo.CurrentCulture);
        int tomorrowsDate = Convert.ToInt32(tomorrowsYear + tomorrowsMonth + tomorrowsDay, CultureInfo.CurrentCulture);
        return tomorrowsDate;
    }

    private void MockDataInContext()
    {
        this.context.Cita.AddRange(new Cita
        {
            dia = GetIntDate(2),
            email = "asd@as",
            hora = 1.1M,
            idCita = 1,
            idTurno = 1,
            nombre = "as",
            telefono = "123",
        });
        this.context.Cita.Add(new Cita
        {
            dia = GetIntDate(-8),
            email = "asd@as",
            hora = 1.1M,
            idCita = 2,
            idTurno = 1,
            nombre = "as",
            telefono = "123",
        });
        this.context.Turno.Add(new Turno
        {
            idTurno = 1,
            idHospital = 1,
            dia = 3,
            inicio = 1,
            fin = 2,
            paralelas = 2,
            porhora = 3,
        });
        this.context.Turno.Add(new Turno
        {
            idTurno = 2,
            idHospital = 1,
            dia = 1356295791,
            inicio = 1,
            fin = 2,
            paralelas = 2,
            porhora = 3,
        });
        this.context.DatosProfesionales.Add(new DatosProfesionales
        {
            idDatosPro = 1,
            nombre = "somehospitalname",
        });

        this.context.SaveChanges();
    }
}
