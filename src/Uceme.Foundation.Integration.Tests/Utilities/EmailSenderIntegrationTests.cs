namespace Uceme.Foundation.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uceme.Foundation.Tools;
    using Uceme.Foundation.Utilities;
    using Uceme.Model.Settings;

    [TestClass]
    public class EmailSenderIntegrationTests
    {
        private readonly string emailTo = "test.uceme@gmail.com";

        [TestMethod]
        [TestCategory("IntegrationTests")]
        public async Task SendEmailToOneAddressAsync()
        {
            //// ARRANGE
            var appSettings = new AuthMessageSenderSettings()
            {
                EmailFrom = "test.uceme@gmail.com",
                HostSmtp = "smtp.gmail.com",
                PortSmtp = 587,
                CredentialUser = "test.uceme@gmail.com",
                CredentialPassword = await AesDecrypt.DecryptAsync("25A9J9p/btpgu9W8TChSQ==").ConfigureAwait(false),
            };

            IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);
            var smtpClient = new SmtpClientWrapper();
            var sut = new EmailSender(options, smtpClient);
            var email = this.emailTo;
            var subject = "Integration Test";
            var htmlMessage = "<p>test</p>";

            //// ACT
            await sut.SendEmailAsync(email, subject, htmlMessage).ConfigureAwait(false);

            //// ASSERT
            smtpClient.Dispose();
        }

        [TestMethod]
        [TestCategory("IntegrationTests")]
        public async Task SendEmailToMoreThanOneAddressAsync()
        {
            //// ARRANGE
            var appSettings = new AuthMessageSenderSettings()
            {
                EmailFrom = "test.uceme@gmail.com",
                HostSmtp = "smtp.gmail.com",
                PortSmtp = 587,
                CredentialUser = "test.uceme@gmail.com",
                CredentialPassword = await AesDecrypt.DecryptAsync("25A9J9p/btpgu9W8TChSQ==").ConfigureAwait(false),
            };

            IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);
            var smtpClient = new SmtpClientWrapper();
            var sut = new EmailSender(options, smtpClient);
            var emails = new List<string>()
            {
                this.emailTo,
                "test.uceme+test@gmail.com",
            };

            var subject = "Integration Test";
            var htmlMessage = "<p>test</p>";

            //// ACT
            await sut.SendEmailAsync(emails, subject, htmlMessage).ConfigureAwait(false);

            //// ASSERT
            smtpClient.Dispose();
        }

        [TestMethod]
        [TestCategory("IntegrationTests")]
        public async Task SendEmailToOneAddressNoAsync()
        {
            //// ARRANGE
            var appSettings = new AuthMessageSenderSettings()
            {
                EmailFrom = "test.uceme@gmail.com",
                HostSmtp = "smtp.gmail.com",
                PortSmtp = 587,
                CredentialUser = "test.uceme@gmail.com",
                CredentialPassword = await AesDecrypt.DecryptAsync("25A9J9p/btpgu9W8TChSQ==").ConfigureAwait(false),
            };

            IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);
            var smtpClient = new SmtpClientWrapper();
            var sut = new EmailSender(options, smtpClient);
            var email = this.emailTo;
            var subject = "Integration Test";
            var htmlMessage = "<p>test</p>";

            //// ACT
            sut.SendEmail(email, subject, htmlMessage);

            //// ASSERT
            smtpClient.Dispose();
        }

        [TestMethod]
        [TestCategory("IntegrationTests")]
        public async Task SendEmailToMoreThanOneAddressNoAsync()
        {
            //// ARRANGE
            var appSettings = new AuthMessageSenderSettings()
            {
                EmailFrom = "test.uceme@gmail.com",
                HostSmtp = "smtp.gmail.com",
                PortSmtp = 587,
                CredentialUser = "test.uceme@gmail.com",
                CredentialPassword = await AesDecrypt.DecryptAsync("25A9J9p/btpgu9W8TChSQ==").ConfigureAwait(false),
            };

            IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);
            var smtpClient = new SmtpClientWrapper();
            var sut = new EmailSender(options, smtpClient);
            var emails = new List<string>()
            {
                this.emailTo,
                "test.uceme+test@gmail.com",
            };

            var subject = "Integration Test";
            var htmlMessage = "<p>test</p>";

            //// ACT
            sut.SendEmail(emails, subject, htmlMessage);

            //// ASSERT
            smtpClient.Dispose();
        }
    }
}
