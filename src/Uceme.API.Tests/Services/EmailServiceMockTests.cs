namespace Uceme.API.Tests;

using System.Threading.Tasks;
using AutoMoqCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uceme.Library.Services;
using Uceme.Model.Settings;

[TestClass]
public class EmailServiceMockTests
{
    [TestMethod]
    [TestCategory("MockTests")]
    public async Task SendEmailToManagementAsync()
    {
        //// ARRANGE
        AutoMoqer mocker = new AutoMoqer();

        AuthMessageSenderSettings appSettings = new AuthMessageSenderSettings()
        {
            EmailFrom = "test.something@verydummything.com",
            HostSmtp = "smtp.gmail.com",
            PortSmtp = 587,
            CredentialUser = "something@verydummything.com",
            CredentialPassword = "dummypassw",
        };

        IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);
        mocker.SetInstance<IOptions<AuthMessageSenderSettings>>(options);

        EmailService sut = mocker.Resolve<EmailService>();

        //// ACT
        bool result = await sut.SendEmailToManagementAsync("to", "sub", "body").ConfigureAwait(false);

        //// ASSERT
        Assert.IsNotNull(result);
        Assert.IsTrue(result);
    }

    [TestMethod]
    [TestCategory("MockTests")]
    public void SendEmailToManagement()
    {
        //// ARRANGE
        AutoMoqer mocker = new AutoMoqer();

        AuthMessageSenderSettings appSettings = new AuthMessageSenderSettings()
        {
            EmailFrom = "test.something@verydummything.com",
            HostSmtp = "smtp.gmail.com",
            PortSmtp = 587,
            CredentialUser = "something@verydummything.com",
            CredentialPassword = "dummypassw",
        };

        IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);
        mocker.SetInstance<IOptions<AuthMessageSenderSettings>>(options);

        EmailService sut = mocker.Resolve<EmailService>();

        //// ACT
        bool result = sut.SendEmailToManagement("to", "sub", "body");

        //// ASSERT
        Assert.IsNotNull(result);
        Assert.IsTrue(result);
    }

    [TestMethod]
    [TestCategory("MockTests")]
    public async Task SendEmailToClientAsync()
    {
        //// ARRANGE
        AutoMoqer mocker = new AutoMoqer();

        AuthMessageSenderSettings appSettings = new AuthMessageSenderSettings()
        {
            EmailFrom = "test.something@verydummything.com",
            HostSmtp = "smtp.gmail.com",
            PortSmtp = 587,
            CredentialUser = "something@verydummything.com",
            CredentialPassword = "dummypassw",
        };

        IOptions<AuthMessageSenderSettings> options = Options.Create(appSettings);
        mocker.SetInstance<IOptions<AuthMessageSenderSettings>>(options);

        EmailService sut = mocker.Resolve<EmailService>();

        //// ACT
        bool result = await sut.SendEmailToClientAsync("to", "sub", "body").ConfigureAwait(false);

        //// ASSERT
        Assert.IsNotNull(result);
        Assert.IsTrue(result);
    }
}
