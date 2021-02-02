namespace Uceme.Foundation.Tests
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uceme.Foundation.Utilities;
    using Uceme.Model.Settings;

    [TestClass]
    public class EmailSenderIntegrationTests
    {
        private readonly string emailTo = "test.uceme@gmail.com";

        [TestMethod]
        [TestCategory("IntegrationTests")]
        public async Task SendEmail()
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
            var sut = new EmailSender(options);
            var email = this.emailTo;
            var subject = "Integration Test";
            var htmlMessage = "<p>test</p>";

            /// ACT
            await sut.SendEmailAsync(email, subject, htmlMessage).ConfigureAwait(false);

            //// ASSERT
        }
    }
}
