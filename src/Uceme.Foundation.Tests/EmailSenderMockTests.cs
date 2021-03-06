namespace Uceme.Foundation.Tests
{
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uceme.Foundation.Utilities;
    using Uceme.Model.Settings;

    [TestClass]
    public class EmailSenderMockTests
    {
        private readonly string emailTo = "test.uceme@gmail.com";

        [TestMethod]
        [TestCategory("MockTests")]
        public async Task SendEmailToOneAddressAsync()
        {
            //// ARRANGE
            var appSettings = new AuthMessageSenderSettings()
            {
                EmailFrom = "test.uceme@gmail.com",
                HostSmtp = "smtp.gmail.com",
                PortSmtp = 587,
                CredentialUser = "notificaciones.uceme@gmail.com",
                CredentialPassword = "Uceme1975",
            };

            IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);

            var smtpClientMock = new Mock<ISmtpClient>();

            smtpClientMock.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>())).Verifiable();

            var sut = new EmailSender(options, smtpClientMock.Object);
            var email = this.emailTo;
            var subject = "BB";
            var htmlMessage = "CC";

            /// ACT
            await sut.SendEmailAsync(email, subject, htmlMessage).ConfigureAwait(false);

            //// ASSERT
            smtpClientMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("MockTests")]
        public async Task SendEmailToMoreThanOneAddressAsync()
        {
            //// ARRANGE
            var appSettings = new AuthMessageSenderSettings()
            {
                EmailFrom = "test.uceme@gmail.com",
                HostSmtp = "smtp.gmail.com",
                PortSmtp = 587,
                CredentialUser = "notificaciones.uceme@gmail.com",
                CredentialPassword = "Uceme1975",
            };

            IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);

            var smtpClientMock = new Mock<ISmtpClient>();

            smtpClientMock.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>())).Verifiable();

            var sut = new EmailSender(options, smtpClientMock.Object);
            var emails = new List<string>() {
                this.emailTo,
                "test.uceme+test@gmail.com"
            };
            var subject = "BB";
            var htmlMessage = "CC";

            /// ACT
            await sut.SendEmailAsync(emails, subject, htmlMessage).ConfigureAwait(false);

            //// ASSERT
            smtpClientMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("MockTests")]
        public void SendEmailToOneAddress()
        {
            //// ARRANGE
            var appSettings = new AuthMessageSenderSettings()
            {
                EmailFrom = "test.uceme@gmail.com",
                HostSmtp = "smtp.gmail.com",
                PortSmtp = 587,
                CredentialUser = "notificaciones.uceme@gmail.com",
                CredentialPassword = "Uceme1975",
            };

            IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);

            var smtpClientMock = new Mock<ISmtpClient>();

            smtpClientMock.Setup(x => x.Send(It.IsAny<MailMessage>())).Verifiable();

            var sut = new EmailSender(options, smtpClientMock.Object);
            var email = this.emailTo;
            var subject = "BB";
            var htmlMessage = "CC";

            /// ACT
            sut.SendEmail(email, subject, htmlMessage);

            //// ASSERT
            smtpClientMock.VerifyAll();
        }

        [TestMethod]
        [TestCategory("MockTests")]
        public void SendEmailToMoreThanOneAddress()
        {
            //// ARRANGE
            var appSettings = new AuthMessageSenderSettings()
            {
                EmailFrom = "test.uceme@gmail.com",
                HostSmtp = "smtp.gmail.com",
                PortSmtp = 587,
                CredentialUser = "notificaciones.uceme@gmail.com",
                CredentialPassword = "Uceme1975",
            };

            IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);

            var smtpClientMock = new Mock<ISmtpClient>();

            smtpClientMock.Setup(x => x.Send(It.IsAny<MailMessage>())).Verifiable();

            var sut = new EmailSender(options, smtpClientMock.Object);
            var emails = new List<string>() {
                this.emailTo,
                "test.uceme+test@gmail.com"
            };
            var subject = "BB";
            var htmlMessage = "CC";

            /// ACT
            sut.SendEmail(emails, subject, htmlMessage);

            //// ASSERT
            smtpClientMock.VerifyAll();
        }
    }
}
