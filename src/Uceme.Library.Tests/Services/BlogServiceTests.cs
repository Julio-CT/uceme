namespace Uceme.Library.Tests.Services;

using System;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.Library.Services;
using Uceme.Model.Data;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;

[TestClass]
public class BlogServiceTests
{
    private BlogService testClass;
    private Mock<ILogger<BlogService>> logger;
    private ApplicationDbContext context;

    [TestInitialize]
    public void SetUp()
    {
        this.logger = new Mock<ILogger<BlogService>>();
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: RandomString(12))
            .Options;
        this.context = new ApplicationDbContext(options, new OperationalStoreOptionsMigrations());
        this.MockDataInContext();

        this.testClass = new BlogService(this.logger.Object, this.context);
    }

    [TestMethod]
    public void CanConstruct()
    {
        // Act
        BlogService instance = new BlogService(this.logger.Object, this.context);

        // Assert
        Assert.IsNotNull(instance);
    }

    [TestMethod]
    public void CannotConstructWithNullLogger()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new BlogService(default, this.context));
    }

    [TestMethod]
    public void CannotConstructWithNullContext()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new BlogService(this.logger.Object, default));
    }

    [TestMethod]
    public void CanCallGetBlogSubset()
    {
        // Arrange
        int amount = 1;
        int page = 2;

        // Act
        System.Collections.Generic.IEnumerable<Blog> result = this.testClass.GetBlogSubset(amount, page);

        // Assert
        Assert.IsInstanceOfType(result, typeof(System.Collections.Generic.IEnumerable<Blog>));
        Assert.AreEqual(amount, result.Count());
    }

    [TestMethod]
    public void CanCallGetAllPosts()
    {
        // Act
        System.Collections.Generic.IEnumerable<Blog> result = this.testClass.GetAllPosts();

        // Assert
        Assert.IsInstanceOfType(result, typeof(System.Collections.Generic.IEnumerable<Blog>));
        Assert.AreEqual(11, result.Count());
    }

    [TestMethod]
    public void CanCallGetPost()
    {
        // Arrange
        string slug = "TestValue1798085746";

        // Act
        Blog result = this.testClass.GetPost(slug);

        // Assert
        Assert.IsInstanceOfType(result, typeof(Blog));
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void CannotCallGetPostWithInvalidSlug(string value)
    {
        Assert.ThrowsException<ArgumentNullException>(() => this.testClass.GetPost(value));
    }

    [TestMethod]
    public void GetPostPerformsMapping()
    {
        // Arrange
        string slug = "TestValue912508891";

        // Act
        Blog result = this.testClass.GetPost(slug);

        // Assert
        Assert.AreSame(slug, result.slug);
    }

    [TestMethod]
    public void CanCallDeletePost()
    {
        // Arrange
        int postId = 690899952;

        // Act
        bool result = this.testClass.DeletePost(postId);

        // Assert
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CanCallUpdatePost()
    {
        // Arrange
        PostRequest blog = new PostRequest
        {
            IdBlog = 590719915,
            Titulo = "TestValue1700396676",
            Fecha = default(DateTime).ToString(CultureInfo.CurrentCulture),
            Foto = "TestValue732173553",
            Texto = "TestValue2116952411",
            Slug = "TestValue1405500024",
            SeoTitle = "TestValue1750291810",
            MetaDescription = "TestValue1627176000",
        };

        // Act
        bool result = this.testClass.UpdatePost(blog);

        // Assert
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CannotCallUpdatePostWithNullBlog()
    {
        Assert.ThrowsException<ArgumentNullException>(() => this.testClass.UpdatePost(default));
    }

    [TestMethod]
    public void CanCallAddPost()
    {
        // Arrange
        PostRequest blog = new PostRequest
        {
            IdBlog = 538705676,
            Titulo = "TestValue363110734",
            Fecha = default(DateTime).ToString(CultureInfo.CurrentCulture),
            Foto = "TestValue844761095",
            Texto = "TestValue1364494516",
            Slug = "TestValue1757813173",
            SeoTitle = "TestValue252985828",
            MetaDescription = "TestValue1839664876",
        };

        // Act
        bool result = this.testClass.AddPost(blog);

        // Assert
        Assert.IsInstanceOfType(result, typeof(bool));
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CannotCallAddPostWithNullBlog()
    {
        Assert.ThrowsException<ArgumentNullException>(() => this.testClass.AddPost(default));
    }

    [TestMethod]
    public void CanCallGetNextPostImage()
    {
        // Act
        string result = this.testClass.GetNextPostImage();

        // Assert
        Assert.IsInstanceOfType(result, typeof(string));
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
        this.context.Blog.AddRange(
            new Blog
            {
                idBlog = 1,
                titulo = "TestValue363110734",
                fecha = default,
                foto = "TestValue1364494516",
                texto = "TestValue1364494123",
                profesional = true,
                slug = "TestValue136449412",
                seoTitle = "TestValue1364423",
                metaDescription = "TestValue1364414516",
                idUsuario = 1,
            },
            new Blog
            {
                idBlog = 2,
                titulo = "TestValue363110734",
                fecha = default,
                foto = "TestValue1364494516",
                texto = "TestValue1364494123",
                profesional = true,
                slug = "TestValue136449412",
                seoTitle = "TestValue1364423",
                metaDescription = "TestValue1364414516",
                idUsuario = 1,
            },
            new Blog
            {
                idBlog = 3,
                titulo = "TestValue363110734",
                fecha = default,
                foto = "TestValue1364494516",
                texto = "TestValue1364494123",
                profesional = true,
                slug = "TestValue1798085746",
                seoTitle = "TestValue1364423",
                metaDescription = "TestValue1364414516",
                idUsuario = 2,
            },
            new Blog
            {
                idBlog = 4,
                titulo = "TestValue363110734",
                fecha = default,
                foto = "TestValue1364494516",
                texto = "TestValue1364494123",
                profesional = true,
                slug = "TestValue1798085746",
                seoTitle = "TestValue1364423",
                metaDescription = "TestValue1364414516",
                idUsuario = 2,
            },
            new Blog
            {
                idBlog = 5,
                titulo = "TestValue363110734",
                fecha = default,
                foto = "TestValue1364494516",
                texto = "TestValue1364494123",
                profesional = true,
                slug = "TestValue1798085746",
                seoTitle = "TestValue1364423",
                metaDescription = "TestValue1364414516",
                idUsuario = 2,
            },
            new Blog
            {
                idBlog = 6,
                titulo = "TestValue363110734",
                fecha = default,
                foto = "TestValue1364494516",
                texto = "TestValue1364494123",
                profesional = true,
                slug = "TestValue1798085746",
                seoTitle = "TestValue1364423",
                metaDescription = "TestValue1364414516",
                idUsuario = 2,
            },
            new Blog
            {
                idBlog = 7,
                titulo = "TestValue363110734",
                fecha = default,
                foto = "TestValue1364494516",
                texto = "TestValue1364494123",
                profesional = true,
                slug = "TestValue1798085746",
                seoTitle = "TestValue1364423",
                metaDescription = "TestValue1364414516",
                idUsuario = 2,
            },
            new Blog
            {
                idBlog = 8,
                titulo = "TestValue363110734",
                fecha = default,
                foto = "TestValue1364494516",
                texto = "TestValue1364494123",
                profesional = true,
                slug = "TestValue1798085746",
                seoTitle = "TestValue1364423",
                metaDescription = "TestValue1364414516",
                idUsuario = 2,
            },
            new Blog
            {
                idBlog = 9,
                titulo = "TestValue363110734",
                fecha = default,
                foto = "TestValue1364494516",
                texto = "TestValue1364494123",
                profesional = true,
                slug = "TestValue912508891",
                seoTitle = "TestValue1364423",
                metaDescription = "TestValue1364414516",
                idUsuario = 2,
            },
            new Blog
            {
                idBlog = 590719915,
                titulo = "TestValue363110734",
                fecha = default,
                foto = "TestValue1364494516",
                texto = "TestValue1364494123",
                profesional = true,
                slug = "TestValue1798085746",
                seoTitle = "TestValue1364423",
                metaDescription = "TestValue1364414516",
                idUsuario = 2,
            },
            new Blog
            {
                idBlog = 690899952,
                titulo = "TestValue363110734",
                fecha = default,
                foto = "TestValue1364494516",
                texto = "TestValue1364494123",
                profesional = true,
                slug = "TestValue1798085746",
                seoTitle = "TestValue1364423",
                metaDescription = "TestValue1364414516",
                idUsuario = 2,
            });

        this.context.SaveChanges();
    }
}
