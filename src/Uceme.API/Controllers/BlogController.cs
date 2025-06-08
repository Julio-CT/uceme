namespace Uceme.API.Controllers;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using Uceme.Library.Services;
using Uceme.Model.DataContracts;
using Uceme.Model.Models;
using Uceme.Model.Settings;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BlogController : Controller
{
    private readonly ILogger<BlogController> logger;
    private readonly IOptions<AppSettings> configuration;
    private readonly IBlogService blogService;

    public BlogController(
        IBlogService blogService,
        IOptions<AppSettings> configuration,
        ILogger<BlogController> logger)
    {
        this.blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("getblogsubset")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Blog>> GetBlogSubset(int amount)
    {
        try
        {
            var result = this.blogService.GetBlogSubset(amount);
            return this.Ok(result.ToList());
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving blog subset with amount {Amount}", amount);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve blog posts",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving blog subset with amount {Amount}", amount);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving blog posts",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    [HttpGet("getbloglist")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Blog>> GetBlogList(int page = 1)
    {
        try
        {
            var result = this.blogService.GetBlogSubset(page == 1 ? 10 : 12, page);
            return this.Ok(result.ToList());
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving blog list for page {Page}", page);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve blog list",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving blog list for page {Page}", page);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving blog list",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    [HttpGet("getallposts")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Blog>> GetAllPosts()
    {
        try
        {
            var result = this.blogService.GetAllPosts();
            return this.Ok(result.ToList());
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving all blog posts");
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve all blog posts",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving all blog posts");
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving all blog posts",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    [HttpGet("getpost")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Blog> GetPost(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Blog post slug is required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        try
        {
            var result = this.blogService.GetPost(slug);
            if (result == null)
            {
                return this.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Blog post with slug '{slug}' not found",
                    Status = StatusCodes.Status404NotFound,
                });
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error retrieving blog post with slug {Slug}", slug);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to retrieve blog post",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error retrieving blog post with slug {Slug}", slug);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while retrieving the blog post",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    [HttpDelete("deletepost/{postId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> DeletePost(int postId)
    {
        try
        {
            var result = this.blogService.DeletePost(postId);
            if (!result)
            {
                return this.NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Blog post with ID {postId} not found",
                    Status = StatusCodes.Status404NotFound,
                });
            }

            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(ex, "Error deleting blog post {PostId}", postId);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = "Unable to delete blog post",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error deleting blog post {PostId}", postId);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while deleting the blog post",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    [HttpPost("addpost")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> AddPost([FromBody] PostRequest postRequest)
    {
        if (postRequest == null)
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Post request data is required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        try
        {
            var result = postRequest.IdBlog != 0 ?
                this.blogService.UpdatePost(postRequest) :
                this.blogService.AddPost(postRequest);
            return this.Ok(result);
        }
        catch (DataException ex)
        {
            this.logger.LogError(
                ex,
                "Error {Action} blog post {PostId}",
                postRequest.IdBlog != 0 ? "updating" : "adding",
                postRequest.IdBlog);
            return this.BadRequest(new ProblemDetails
            {
                Title = "Database Error",
                Detail = $"Unable to {(postRequest.IdBlog != 0 ? "update" : "add")} blog post",
                Status = StatusCodes.Status400BadRequest,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(
                ex,
                "Unexpected error {Action} blog post {PostId}",
                postRequest.IdBlog != 0 ? "updating" : "adding",
                postRequest.IdBlog);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = $"An unexpected error occurred while {(postRequest.IdBlog != 0 ? "updating" : "adding")} the blog post",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    [HttpPost("onpostuploadasync")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<string>> OnPostUploadAsync(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "File is required and must not be empty",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        string[] allowedImageTypes = new string[] { "IMAGE/JPEG", "IMAGE/PNG" };
        if (!allowedImageTypes.Contains(file.ContentType.ToUpperInvariant())
            || Path.GetExtension(file.FileName).Length >= 6)
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid File Format",
                Detail = "File must be a JPEG or PNG image",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        if (this.configuration?.Value?.BlogImagesDir == null
            || !Directory.Exists(this.configuration?.Value?.BlogImagesDir))
        {
            return this.StatusCode(StatusCodes.Status502BadGateway, new ProblemDetails
            {
                Title = "Configuration Error",
                Detail = "Blog images directory is not properly configured",
                Status = StatusCodes.Status502BadGateway,
            });
        }

        try
        {
            string blogImagesFolder = this.configuration.Value.BlogImagesDir;
            string filename = "Blog" + this.blogService.GetNextPostImage();
            filename += Path.GetExtension(file.FileName);

#pragma warning disable CA3003 // Review code for file path injection vulnerabilities
            // SaveToOriginalFormat(file, filename, blogImagesFolder);
            var webPFileName = await SaveToWebP(file, filename, blogImagesFolder).ConfigureAwait(false);
#pragma warning restore CA3003 // Review code for file path injection vulnerabilities

            return this.Ok(webPFileName);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error uploading file {FileName}", file.FileName);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while uploading the file",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }

    private static async Task<string> SaveToWebP(IFormFile file, string filename, string blogImagesFolder)
    {
        string webPFileName = filename + ".webp";
        string webPImagePath = Path.Combine(blogImagesFolder, webPFileName);
        using (FileStream webpst = System.IO.File.Create(webPImagePath))
        {
            using (Image someImage = await Image.LoadAsync(file.OpenReadStream()).ConfigureAwait(false))
            {
                await someImage.SaveAsWebpAsync(webpst).ConfigureAwait(false);
            }
        }

        return webPFileName;
    }
}
