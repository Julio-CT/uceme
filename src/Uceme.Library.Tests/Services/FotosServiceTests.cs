namespace Uceme.Library.Tests.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.Library.Services;
using Uceme.Model.Data;
using Uceme.Model.Models;

[TestClass]
public class FotosServiceTests
{
    private FotosService testClass;
    private Mock<ILogger<FotosService>> logger;
    private ApplicationDbContext context;

    [TestInitialize]
    public void SetUp()
    {
        this.logger = new Mock<ILogger<FotosService>>();
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: RandomString(12))
            .Options;
        this.context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());
        this.MockDataInContext();
        this.testClass = new FotosService(this.logger.Object, this.context);
    }

    [TestMethod]
    public void CanConstruct()
    {
        // Act
        FotosService instance = new FotosService(this.logger.Object, this.context);

        // Assert
        Assert.IsNotNull(instance);
    }

    [TestMethod]
    public void CannotConstructWithNullLogger()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new FotosService(default, this.context));
    }

    [TestMethod]
    public void CannotConstructWithNullContext()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new FotosService(this.logger.Object, default));
    }

    [TestMethod]
    public void CanCallGetFotos()
    {
        // Act
        IEnumerable<Foto> result = this.testClass.GetFotos();

        // Assert
        Assert.IsInstanceOfType(result, typeof(IEnumerable<Foto>));
        Assert.AreEqual(1, result.Count());
    }

    private static string RandomString(int length)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private void MockDataInContext()
    {
        this.context.Fotos.AddRange(new Foto
        {
            idFoto = 2,
            destacada = true,
            posicion = 2,
            nombre = "as",
            texto = "123",
        });

        this.context.SaveChanges();
    }
}
