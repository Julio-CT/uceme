namespace Uceme.API.Tests.Controllers;

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.API.Controllers;
using Uceme.Model.Settings;

[TestClass]
public class SettingsControllerTests
{
    private SettingsController testClass;
    private Mock<ILogger<SettingsController>> logger;
    private Mock<IOptions<AppSettings>> configuration;

    [TestInitialize]
    public void SetUp()
    {
        this.logger = new Mock<ILogger<SettingsController>>();
        this.configuration = new Mock<IOptions<AppSettings>>();
        this.testClass = new SettingsController(this.logger.Object, this.configuration.Object);
    }

    [TestMethod]
    public void CanConstruct()
    {
        // Act
        var instance = new SettingsController(this.logger.Object, this.configuration.Object);

        // Assert
        Assert.IsNotNull(instance);
    }

    [TestMethod]
    public void CannotConstructWithNullLogger()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new SettingsController(default, this.configuration.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullConfiguration()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new SettingsController(this.logger.Object, default));
    }

    [TestMethod]
    public void CanCallGetSettings()
    {
        // Act
        var result = this.testClass.GetSettings();

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<Dictionary<string, object>>));
        Assert.AreEqual(string.Empty, result.Value?["telephone"]);
        Assert.AreEqual(string.Empty, result.Value?["address"]);
        Assert.AreEqual(string.Empty, result.Value?["contactEmail"]);
    }
}
