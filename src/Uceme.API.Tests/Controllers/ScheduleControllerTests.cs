namespace Uceme.API.Tests.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.API.Controllers;
using Uceme.Library.Services;
using Uceme.Model.Models;

[TestClass]
public class ScheduleControllerTests
{
    private ScheduleController testClass;
    private Mock<ILogger<ScheduleController>> logger;
    private Mock<IScheduleService> scheduleService;

    [TestInitialize]
    public void SetUp()
    {
        this.logger = new Mock<ILogger<ScheduleController>>();
        this.scheduleService = new Mock<IScheduleService>();
        this.testClass = new ScheduleController(this.logger.Object, this.scheduleService.Object);
    }

    [TestCleanup]
    public void CleanUp()
    {
        this.testClass.Dispose();
    }

    [TestMethod]
    public void CanConstruct()
    {
        // Act
        ScheduleController? instance = new ScheduleController(this.logger.Object, this.scheduleService.Object);

        // Assert
        Assert.IsNotNull(instance);
        instance.Dispose();
    }

    [TestMethod]
    public void CannotConstructWithNullLogger()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new ScheduleController(default, this.scheduleService.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullScheduleService()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new ScheduleController(this.logger.Object, default));
    }

    [TestMethod]
    public void CanCallGetTurns()
    {
        // Arrange
        Turno? turno = new Turno
        {
            idTurno = 915526957,
            dia = 1838506251,
            inicio = 1064507829.66M,
            fin = 170054113.68M,
            paralelas = 187022108,
            porhora = 1624990659,
            idHospital = 15085735,
        };

        this.scheduleService.Setup(mock => mock.GetTurns()).Returns(new[]
        {
            turno,
            new Turno
            {
                idTurno = 1336903815,
                dia = 1608322205,
                inicio = 1067295355.83M,
                fin = 296649392.49M,
                paralelas = 1849082439,
                porhora = 1529351990,
                idHospital = 741244556,
            },
            new Turno
            {
                idTurno = 726191147,
                dia = 1882942165,
                inicio = 744648621.75M,
                fin = 1222295321.61M,
                paralelas = 2112307929,
                porhora = 2087201815,
                idHospital = 620748157,
            },
        });

        // Act
        ActionResult<IEnumerable<Turno>>? result = this.testClass.GetTurns();

        // Assert
        this.scheduleService.Verify(mock => mock.GetTurns());
        Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Turno>>));
        Assert.AreEqual(3, result.Value?.ToArray().Length);
        Assert.AreEqual(turno, result.Value?.ToArray()[0]);
        Assert.AreEqual(1336903815, result.Value?.ToArray()[1].idTurno);
        Assert.AreEqual(1882942165, result.Value?.ToArray()[2].dia);
    }
}
