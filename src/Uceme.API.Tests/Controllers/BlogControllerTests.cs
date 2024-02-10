namespace Uceme.API.Tests.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Uceme.API.Controllers;
using Uceme.Library.Services;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;
using Uceme.Model.Settings;

[TestClass]
public class BlogControllerTests
{
    private BlogController testClass;
    private Mock<IBlogService> blogService;
    private Mock<IOptions<AppSettings>> configuration;
    private Mock<ILogger<BlogController>> logger;

    [TestInitialize]
    public void SetUp()
    {
        this.blogService = new Mock<IBlogService>();
        this.configuration = new Mock<IOptions<AppSettings>>();
        this.logger = new Mock<ILogger<BlogController>>();
        this.testClass = new BlogController(this.blogService.Object, this.configuration.Object, this.logger.Object);
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
        BlogController instance = new BlogController(this.blogService.Object, this.configuration.Object, this.logger.Object);

        // Assert
        Assert.IsNotNull(instance);
        instance.Dispose();
    }

    [TestMethod]
    public void CannotConstructWithNullBlogService()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new BlogController(default, this.configuration.Object, this.logger.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullConfiguration()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new BlogController(this.blogService.Object, default, this.logger.Object));
    }

    [TestMethod]
    public void CannotConstructWithNullLogger()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new BlogController(this.blogService.Object, this.configuration.Object, default));
    }

    [TestMethod]
    public void CanCallGetBlogSubset()
    {
        // Arrange
        int amount = 165524861;

        this.blogService.Setup(mock => mock.GetBlogSubset(It.IsAny<int>(), It.IsAny<int>())).Returns(new[]
        {
            new Blog
            {
                idBlog = 849455759,
                titulo = "TestValue1583563540",
                fecha = DateTime.UtcNow,
                foto = "TestValue1327979143",
                texto = "TestValue1642168174",
                profesional = true,
                slug = "TestValue1034987011",
                seoTitle = "TestValue164024647",
                metaDescription = "TestValue40879565",
                idUsuario = 923502719,
            },
            new Blog
            {
                idBlog = 1275688266,
                titulo = "TestValue572622610",
                fecha = DateTime.UtcNow,
                foto = "TestValue74916662",
                texto = "TestValue1567777783",
                profesional = false,
                slug = "TestValue1979775100",
                seoTitle = "TestValue815653532",
                metaDescription = "TestValue1498323893",
                idUsuario = 430977573,
            },
            new Blog
            {
                idBlog = 1174568865,
                titulo = "TestValue1872993785",
                fecha = DateTime.UtcNow,
                foto = "TestValue1359335425",
                texto = "TestValue92907332",
                profesional = true,
                slug = "TestValue1649352115",
                seoTitle = "TestValue215018285",
                metaDescription = "TestValue993776361",
                idUsuario = 1070164581,
            },
        });

        // Act
        ActionResult<IEnumerable<Blog>> result = this.testClass.GetBlogSubset(amount);

        // Assert
        this.blogService.Verify(mock => mock.GetBlogSubset(It.IsAny<int>(), It.IsAny<int>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Blog>>));
        Assert.AreEqual(3, result.Value?.Count());
    }

    [TestMethod]
    public void CanCallGetBlogList()
    {
        // Arrange
        int page = 756548427;

        this.blogService.Setup(mock => mock.GetBlogSubset(It.IsAny<int>(), It.IsAny<int>())).Returns(new[]
        {
            new Blog
            {
                idBlog = 1047274587,
                titulo = "TestValue74491828",
                fecha = DateTime.UtcNow,
                foto = "TestValue1264824102",
                texto = "TestValue353149938",
                profesional = true,
                slug = "TestValue1046478478",
                seoTitle = "TestValue886967026",
                metaDescription = "TestValue1569726816",
                idUsuario = 776789240,
            },
            new Blog
            {
                idBlog = 1275042333,
                titulo = "TestValue387996939",
                fecha = DateTime.UtcNow,
                foto = "TestValue105353614",
                texto = "TestValue333465380",
                profesional = false,
                slug = "TestValue1872443682",
                seoTitle = "TestValue329885371",
                metaDescription = "TestValue1295869813",
                idUsuario = 678191865,
            },
            new Blog
            {
                idBlog = 771100949,
                titulo = "TestValue1313719286",
                fecha = DateTime.UtcNow,
                foto = "TestValue1798152203",
                texto = "TestValue1327176707",
                profesional = false,
                slug = "TestValue869807874",
                seoTitle = "TestValue1735248580",
                metaDescription = "TestValue82768313",
                idUsuario = 1509061394,
            },
        });

        // Act
        ActionResult<IEnumerable<Blog>> result = this.testClass.GetBlogList(page);

        // Assert
        this.blogService.Verify(mock => mock.GetBlogSubset(It.IsAny<int>(), It.IsAny<int>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Blog>>));
        Assert.AreEqual(3, result.Value?.Count());
    }

    [TestMethod]
    public void CanCallGetAllPosts()
    {
        // Arrange
        this.blogService.Setup(mock => mock.GetAllPosts()).Returns(new[]
        {
            new Blog
            {
                idBlog = 807863901,
                titulo = "TestValue1826110675",
                fecha = DateTime.UtcNow,
                foto = "TestValue1427149889",
                texto = "TestValue488605138",
                profesional = true,
                slug = "TestValue2145983433",
                seoTitle = "TestValue1141088625",
                metaDescription = "TestValue849010891",
                idUsuario = 10864164,
            },
            new Blog
            {
                idBlog = 219472671,
                titulo = "TestValue1284046140",
                fecha = DateTime.UtcNow,
                foto = "TestValue521299305",
                texto = "TestValue198151924",
                profesional = false,
                slug = "TestValue38864292",
                seoTitle = "TestValue223281560",
                metaDescription = "TestValue42980634",
                idUsuario = 1069215251,
            },
            new Blog
            {
                idBlog = 1539528405,
                titulo = "TestValue1342020263",
                fecha = DateTime.UtcNow,
                foto = "TestValue367947297",
                texto = "TestValue445814268",
                profesional = false,
                slug = "TestValue1684310067",
                seoTitle = "TestValue222675412",
                metaDescription = "TestValue1903928942",
                idUsuario = 514856305,
            },
        });

        // Act
        ActionResult<IEnumerable<Blog>> result = this.testClass.GetAllPosts();

        // Assert
        this.blogService.Verify(mock => mock.GetAllPosts());
        Assert.IsInstanceOfType(result, typeof(ActionResult<IEnumerable<Blog>>));
        Assert.AreEqual(3, result.Value?.Count());
    }

    [TestMethod]
    public void CanCallGetPost()
    {
        // Arrange
        string slug = "TestValue1995376091";

        this.blogService.Setup(mock => mock.GetPost(It.IsAny<string>())).Returns(new Blog
        {
            idBlog = 1867581527,
            titulo = "TestValue1030137135",
            fecha = DateTime.UtcNow,
            foto = "TestValue395016228",
            texto = "TestValue765385005",
            profesional = false,
            slug = "TestValue1684900731",
            seoTitle = "TestValue130418599",
            metaDescription = "TestValue761862915",
            idUsuario = 1098162212,
        });

        // Act
        ActionResult<Blog> result = this.testClass.GetPost(slug);

        // Assert
        this.blogService.Verify(mock => mock.GetPost(It.IsAny<string>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<Blog>));
        Assert.AreEqual(1867581527, result.Value?.idBlog);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void CannotCallGetPostWithInvalidSlug(string value)
    {
        // Act
        ActionResult<Blog>? result = this.testClass.GetPost(default);

        // Assert
        Assert.IsInstanceOfType(result, typeof(ActionResult<Blog>));
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public void CanCallDeletePost()
    {
        // Arrange
        int postId = 140531184;

        this.blogService.Setup(mock => mock.DeletePost(It.IsAny<int>())).Returns(false);

        // Act
        ActionResult<bool> result = this.testClass.DeletePost(postId);

        // Assert
        this.blogService.Verify(mock => mock.DeletePost(It.IsAny<int>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.IsFalse(result.Value);
    }

    [TestMethod]
    public void CanCallAddPost()
    {
        // Arrange
        PostRequest postRequest = new PostRequest
        {
            IdBlog = 1023432791,
            Titulo = "TestValue319021207",
            Fecha = "TestValue1076397141",
            Foto = "TestValue1541629372",
            Texto = "TestValue249801644",
            Slug = "TestValue1115567362",
            SeoTitle = "TestValue1388103919",
            MetaDescription = "TestValue1288312415",
        };

        this.blogService.Setup(mock => mock.UpdatePost(It.IsAny<PostRequest>())).Returns(false);
        this.blogService.Setup(mock => mock.AddPost(It.IsAny<PostRequest>())).Returns(false);

        // Act
        ActionResult<bool> result = this.testClass.AddPost(postRequest);

        // Assert
        this.blogService.Verify(mock => mock.UpdatePost(It.IsAny<PostRequest>()));
        this.blogService.Verify(mock => mock.AddPost(It.IsAny<PostRequest>()), times: Times.Never);
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.IsFalse(result.Value);
    }

    [TestMethod]
    public void CanCallAddPostWithUpdateOk()
    {
        // Arrange
        PostRequest postRequest = new PostRequest
        {
            IdBlog = 0,
            Titulo = "TestValue319021207",
            Fecha = "TestValue1076397141",
            Foto = "TestValue1541629372",
            Texto = "TestValue249801644",
            Slug = "TestValue1115567362",
            SeoTitle = "TestValue1388103919",
            MetaDescription = "TestValue1288312415",
        };

        this.blogService.Setup(mock => mock.UpdatePost(It.IsAny<PostRequest>())).Returns(true);
        this.blogService.Setup(mock => mock.AddPost(It.IsAny<PostRequest>())).Returns(false);

        // Act
        ActionResult<bool> result = this.testClass.AddPost(postRequest);

        // Assert
        this.blogService.Verify(mock => mock.UpdatePost(It.IsAny<PostRequest>()), times: Times.Never);
        this.blogService.Verify(mock => mock.AddPost(It.IsAny<PostRequest>()));
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.IsFalse(result.Value);
    }

    [TestMethod]
    public void CannotCallAddPostWithNullPostRequest()
    {
        // Arrange
        this.blogService.Setup(mock => mock.AddPost(It.IsAny<PostRequest>())).Returns(false);

        // Act
        ActionResult<bool> result = this.testClass.AddPost(null);

        // Assert
        this.blogService.Verify(mock => mock.DeletePost(It.IsAny<int>()), times: Times.Never);
        Assert.IsInstanceOfType(result, typeof(ActionResult<bool>));
        Assert.IsFalse(result.Value);
    }

    [TestMethod]
    public async Task CannotCallOnPostUploadAsyncWithNullFile()
    {
        // Arrange
        // Act
        ActionResult<string> result = await this.testClass.OnPostUploadAsync(default).ConfigureAwait(false);

        // Assert
        this.blogService.Verify(mock => mock.DeletePost(It.IsAny<int>()), times: Times.Never);
        Assert.IsInstanceOfType(result, typeof(ActionResult<string>));
    }
}
