﻿namespace Uceme.Foundation.Tests
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
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

        private readonly string encryptPass;

        public EmailSenderIntegrationTests()
        {
            this.encryptPass = "q25A9J9p/btpgu9W8TChSQ==";
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.encryptPass = "Rqm8TCkm8iB9uKj0oti+Uj7Y1IHPpVD9jZdummMgUgE=";
            }
        }

        [TestMethod]
        [TestCategory("IntegrationTests")]
        public async Task SendEmailToOneAddressAsync()
        {
            //// ARRANGE
            var appSettings = new AuthMessageSenderSettings()
            {
                EmailFrom = this.emailTo,
                HostSmtp = "smtp.gmail.com",
                PortSmtp = 587,
                CredentialUser = this.emailTo,
                CredentialPassword = await AesDecrypt.DecryptAsync(this.encryptPass).ConfigureAwait(false),
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
                EmailFrom = this.emailTo,
                HostSmtp = "smtp.gmail.com",
                PortSmtp = 587,
                CredentialUser = this.emailTo,
                CredentialPassword = await AesDecrypt.DecryptAsync(this.encryptPass).ConfigureAwait(false),
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
                EmailFrom = this.emailTo,
                HostSmtp = "smtp.gmail.com",
                PortSmtp = 587,
                CredentialUser = this.emailTo,
                CredentialPassword = await AesDecrypt.DecryptAsync(this.encryptPass).ConfigureAwait(false),
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
                EmailFrom = this.emailTo,
                HostSmtp = "smtp.gmail.com",
                PortSmtp = 587,
                CredentialUser = this.emailTo,
                CredentialPassword = await AesDecrypt.DecryptAsync(this.encryptPass).ConfigureAwait(false),
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
