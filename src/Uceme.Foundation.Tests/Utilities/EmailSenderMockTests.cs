namespace Uceme.Foundation.Tests;

using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.Foundation.Tools;
using Uceme.Foundation.Utilities;
using Uceme.Model.Settings;

[TestClass]
public class EmailSenderMockTests
{
    private readonly string emailTo = "test.uceme@gmail.com";

    private readonly string encryptPass;

    public EmailSenderMockTests()
    {
        this.encryptPass = "q25A9J9p/btpgu9W8TChSQ==";
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            this.encryptPass = "25A9J9p/btpgu9W8TChSQ==";
        }
    }

    [TestMethod]
    [TestCategory("MockTests")]
    public async Task SendEmailToOneAddressAsync()
    {
        //// ARRANGE
        AuthMessageSenderSettings appSettings = new AuthMessageSenderSettings()
        {
            EmailFrom = this.emailTo,
            HostSmtp = "smtp.gmail.com",
            PortSmtp = 587,
            CredentialUser = "notificaciones.uceme@gmail.com",
            CredentialPassword = await AesDecrypt.DecryptAsync(this.encryptPass).ConfigureAwait(false),
        };

        IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);

        Mock<ISmtpClient> smtpClientMock = new Mock<ISmtpClient>();

        smtpClientMock.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>())).Verifiable();

        EmailSender sut = new EmailSender(options, smtpClientMock.Object);
        string email = this.emailTo;
        string subject = "BB";
        string htmlMessage = "CC";

        //// ACT
        await sut.SendEmailAsync(email, subject, htmlMessage).ConfigureAwait(false);

        //// ASSERT
        smtpClientMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("MockTests")]
    public async Task SendEmailToMoreThanOneAddressAsync()
    {
        //// ARRANGE
        AuthMessageSenderSettings appSettings = new AuthMessageSenderSettings()
        {
            EmailFrom = this.emailTo,
            HostSmtp = "smtp.gmail.com",
            PortSmtp = 587,
            CredentialUser = "notificaciones.uceme@gmail.com",
            CredentialPassword = await AesDecrypt.DecryptAsync(this.encryptPass).ConfigureAwait(false),
        };

        IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);

        Mock<ISmtpClient> smtpClientMock = new Mock<ISmtpClient>();

        smtpClientMock.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>())).Verifiable();

        EmailSender sut = new EmailSender(options, smtpClientMock.Object);
        List<string> emails = new List<string>()
        {
            this.emailTo,
            "test.uceme+test@gmail.com",
        };
        string subject = "BB";
        string htmlMessage = "CC";

        //// ACT
        await sut.SendEmailAsync(emails, subject, htmlMessage).ConfigureAwait(false);

        //// ASSERT
        smtpClientMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("MockTests")]
    public async Task SendEmailToOneAddress()
    {
        //// ARRANGE
        AuthMessageSenderSettings appSettings = new AuthMessageSenderSettings()
        {
            EmailFrom = this.emailTo,
            HostSmtp = "smtp.gmail.com",
            PortSmtp = 587,
            CredentialUser = "notificaciones.uceme@gmail.com",
            CredentialPassword = await AesDecrypt.DecryptAsync(this.encryptPass).ConfigureAwait(false),
        };

        IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);

        Mock<ISmtpClient> smtpClientMock = new Mock<ISmtpClient>();

        smtpClientMock.Setup(x => x.Send(It.IsAny<MailMessage>())).Verifiable();

        EmailSender sut = new EmailSender(options, smtpClientMock.Object);
        string email = this.emailTo;
        string subject = "BB";
        string htmlMessage = "CC";

        //// ACT
        sut.SendEmail(email, subject, htmlMessage);

        //// ASSERT
        smtpClientMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("MockTests")]
    public async Task SendEmailToMoreThanOneAddress()
    {
        //// ARRANGE
        AuthMessageSenderSettings appSettings = new AuthMessageSenderSettings()
        {
            EmailFrom = this.emailTo,
            HostSmtp = "smtp.gmail.com",
            PortSmtp = 587,
            CredentialUser = "notificaciones.uceme@gmail.com",
            CredentialPassword = await AesDecrypt.DecryptAsync(this.encryptPass).ConfigureAwait(false),
        };

        IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);

        Mock<ISmtpClient> smtpClientMock = new Mock<ISmtpClient>();

        smtpClientMock.Setup(x => x.Send(It.IsAny<MailMessage>())).Verifiable();

        EmailSender sut = new EmailSender(options, smtpClientMock.Object);
        List<string> emails = new List<string>()
        {
            this.emailTo,
            "test.uceme+test@gmail.com",
        };
        string subject = "BB";
        string htmlMessage = "CC";

        //// ACT
        sut.SendEmail(emails, subject, htmlMessage);

        //// ASSERT
        smtpClientMock.VerifyAll();
    }
}
