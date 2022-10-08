namespace Uceme.Library.Tests
{
    using Microsoft.Extensions.Logging;
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
            //////// ARRANGE
            ////var logger = new Mock<ILogger<HospitalService>>();
            ////var context = new Mock<IApplicationDbContext>();
            ////var emailService = new Mock<IEmailService>();
            ////var sut = new AppointmentService(
            ////    logger.Object,
            ////    context.Object,
            ////    emailService.Object);

            //////// ACT
            ////sut.GetAppointments();

            //////// ASSERT
        }
    }
}
