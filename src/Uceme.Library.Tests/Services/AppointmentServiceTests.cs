namespace Uceme.Library.Tests.Services
{
    using System.Linq;
    using AutoMoqCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uceme.Library.Services;
    using Uceme.Model.Data;

    [TestClass]
    public class AppointmentServiceTests
    {
        [TestMethod]
        public void AppointmentService_RightInput_RightOutput()
        {
            //// ARRANGE
            /////create In Memory Database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "EmployeeDataBase")
                .Options;

            //// Create mocked Context by seeding Data as per Schema///
            using (var context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations()))
            {
                context.Cita.Add(new Model.Models.Cita
                {
                    dia = 1,
                    email = "asd@as",
                    hora = 1.1M,
                    idCita = 1,
                    idTurno = 1,
                    nombre = "as",
                    telefono = "123",
                });
                context.Cita.Add(new Model.Models.Cita
                {
                    dia = 1,
                    email = "asd@as",
                    hora = 1.1M,
                    idCita = 2,
                    idTurno = 1,
                    nombre = "as",
                    telefono = "123",
                });
                context.Turno.Add(new Model.Models.Turno
                {
                    idTurno = 1,
                    idHospital = 1,
                });
                context.DatosProfesionales.Add(new Model.Models.DatosProfesionales
                {
                    idDatosPro = 1,
                    nombre = "hospitalname",
                });

                context.SaveChanges();

                var mocker = new AutoMoqer();
                mocker.SetInstance<IApplicationDbContext>(context);

                var sut = mocker.Create<AppointmentService>();

                //// ACT
                var appointments = sut.GetAppointments();

                //// ASSERT
                Assert.IsNotNull(appointments);
                Assert.AreEqual(2, appointments.Count());
            }
        }
    }
}
