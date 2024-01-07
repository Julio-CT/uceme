namespace Uceme.Library.Tests.Services;

using System;
using System.Threading.Tasks;
using AutoMoqCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.Foundation.Utilities;
using Uceme.Library.Services;
using Uceme.Model.Settings;

[TestClass]
public class EmailServiceTests
{
    private EmailService testClass;
    ////private Mock<IOptions<AuthMessageSenderSettings>> optionsAccessor;
    private IOptions<AuthMessageSenderSettings> optionsFake;
    private Mock<ILogger<EmailService>> logger;
    private Mock<IEmailSender> emailSender;

    [TestInitialize]
    public void SetUp()
    {
        AutoMoqer mocker = new AutoMoqer();

        AuthMessageSenderSettings appSettings = new AuthMessageSenderSettings()
        {
            EmailFrom = "test.something@verydummything.com",
            HostSmtp = "smtp.gmail.com",
            PortSmtp = 587,
            CredentialUser = "something@verydummything.com",
            CredentialPassword = "dummypassw",
        };

        this.optionsFake = Options.Create(appSettings);
        mocker.SetInstance(this.optionsFake);

        this.testClass = mocker.Resolve<EmailService>();
        this.logger = new Mock<ILogger<EmailService>>();
        this.emailSender = new Mock<IEmailSender>();
    }

    [TestMethod]
    public void CanConstruct()
    {
        // Act
        var instance = new EmailService(this.optionsFake, this.logger.Object, this.emailSender.Object);

        // Assert
        Assert.IsNotNull(instance);
    }

    [TestMethod]
    public void CannotConstructWithNullOptionsAccessor()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new EmailService(default, this.logger.Object, this.emailSender.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullLogger()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new EmailService(this.optionsFake, default, this.emailSender.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullEmailSender()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new EmailService(this.optionsFake, this.logger.Object, default));
    }

    [TestMethod]
    public async Task CanCallSendEmailToManagementAsync()
    {
        // Arrange
        var fromAddress = "Test@Value.22";
        var subject = "TestValue112382731";
        var body = "TestValue2082826035";

        // Act
        var result = await this.testClass.SendEmailToManagementAsync(fromAddress, subject, body).ConfigureAwait(false);

        // Assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("asfas@@asdfas.es")]
    [DataRow("adasd.es")]
    [DataRow("   ")]
    public async Task CannotCallSendEmailToManagementAsyncWithInvalidFromAddress(string value)
    {
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => this.testClass.SendEmailToManagementAsync(value, "TestValue1815585287", "TestValue335517980")).ConfigureAwait(false);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("·$%·$%· ")]
    public async Task CannotCallSendEmailToManagementAsyncWithInvalidSubject(string value)
    {
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => this.testClass.SendEmailToManagementAsync("TestValue1405267161", value, "TestValue1749340446")).ConfigureAwait(false);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public async Task CannotCallSendEmailToManagementAsyncWithInvalidBody(string value)
    {
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => this.testClass.SendEmailToManagementAsync("TestValue1972857789", "TestValue52240965", value)).ConfigureAwait(false);
    }

    [TestMethod]
    public void CanCallSendEmailToManagement()
    {
        // Arrange
        var fromAddress = "Test@Value.22";
        var subject = "TestValue649043987";
        var body = "TestValue359222217";

        // Act
        var result = this.testClass.SendEmailToManagement(fromAddress, subject, body);

        // Assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("asfas@@asdfas.es")]
    [DataRow("adasd.es")]
    [DataRow("   ")]
    public void CannotCallSendEmailToManagementWithInvalidFromAddress(string value)
    {
        Assert.ThrowsException<ArgumentException>(() => this.testClass.SendEmailToManagement(value, "TestValue382143629", "TestValue75755626"));
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("·$%·$%· ")]
    public void CannotCallSendEmailToManagementWithInvalidSubject(string value)
    {
        Assert.ThrowsException<ArgumentException>(() => this.testClass.SendEmailToManagement("TestValue383687830", value, "TestValue619735923"));
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void CannotCallSendEmailToManagementWithInvalidBody(string value)
    {
        Assert.ThrowsException<ArgumentException>(() => this.testClass.SendEmailToManagement("TestValue1588817604", "TestValue1350485477", value));
    }

    [TestMethod]
    public async Task CanCallSendEmailToClientAsync()
    {
        // Arrange
        var toAddress = "Test@Value.22";
        var subject = "TestValue370018525";
        var body = "TestValue1231448036";

        // Act
        var result = await this.testClass.SendEmailToClientAsync(toAddress, subject, body).ConfigureAwait(false);

        // Assert
        Assert.IsTrue(result);
    }

    [DataTestMethod]
    [DataRow("asfas@@asdfas.es")]
    [DataRow("adasd.es")]
    [DataRow("   ")]
    public async Task CannotCallSendEmailToClientAsyncWithInvalidToAddress(string value)
    {
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => this.testClass.SendEmailToClientAsync(value, "TestValue1445224650", "TestValue106845759")).ConfigureAwait(false);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("·$%·$%· ")]
    public async Task CannotCallSendEmailToClientAsyncWithInvalidSubject(string value)
    {
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => this.testClass.SendEmailToClientAsync("TestValue567195111", value, "TestValue898403055")).ConfigureAwait(false);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public async Task CannotCallSendEmailToClientAsyncWithInvalidBody(string value)
    {
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => this.testClass.SendEmailToClientAsync("TestValue1068896686", "TestValue1967059523", value)).ConfigureAwait(false);
    }

    [TestMethod]
    public void CanGetOptions()
    {
        // Assert
        Assert.IsInstanceOfType(this.testClass.Options, typeof(AuthMessageSenderSettings));
    }

    [TestMethod]
    public async Task SendEmailToManagementAsync()
    {
        //// ACT
        bool result = await this.testClass.SendEmailToManagementAsync("to@me.com", "sub", "body").ConfigureAwait(false);

        //// ASSERT
        Assert.IsNotNull(result);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void SendEmailToManagement()
    {
        //// ACT
        bool result = this.testClass.SendEmailToManagement("to@me.com", "sub", "body");

        //// ASSERT
        Assert.IsNotNull(result);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task SendEmailToClientAsync()
    {
        //// ACT
        bool result = await this.testClass.SendEmailToClientAsync("to@me.com", "sub", "body").ConfigureAwait(false);

        //// ASSERT
        Assert.IsNotNull(result);
        Assert.IsTrue(result);
    }
}
