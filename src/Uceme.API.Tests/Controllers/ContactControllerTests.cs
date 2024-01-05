namespace Uceme.API.Tests.Controllers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.API.Controllers;
using Uceme.Library.Services;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;

[TestClass]
public class ContactControllerTests
{
    private ContactController testClass;
    private Mock<ILogger<ContactController>> logger;
    private Mock<IEmailService> emailService;

    [TestInitialize]
    public void SetUp()
    {
        this.logger = new Mock<ILogger<ContactController>>();
        this.emailService = new Mock<IEmailService>();
        this.testClass = new ContactController(this.logger.Object, this.emailService.Object);
    }

    [TestMethod]
    public void CanConstruct()
    {
        // Act
        var instance = new ContactController(this.logger.Object, this.emailService.Object);

        // Assert
        Assert.IsNotNull(instance);
    }

    [TestMethod]
    public void CannotConstructWithNullLogger()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new ContactController(default, this.emailService.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullEmailService()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new ContactController(this.logger.Object, default));
    }

    [TestMethod]
    public async Task CanCallContactEmailAsync()
    {
        // Arrange
        var message = new EmailMessage
        {
            Name = "TestValue1472628373",
            Email = "TestValue1890323469",
            Message = "TestValue265929726",
            Loaded = true,
        };

        this.emailService.Setup(mock => mock.SendEmailToManagementAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

        // Act
        var result = await this.testClass.ContactEmailAsync(message).ConfigureAwait(false);

        // Assert
        this.emailService.Verify(mock => mock.SendEmailToManagementAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.IsTrue(result.Value);
    }

    [TestMethod]
    public async Task CannotCallContactEmailAsyncWithNullMessage()
    {
        // Act
        var result = await this.testClass.ContactEmailAsync(default).ConfigureAwait(false);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        Assert.IsFalse(result.Value);
    }
}
