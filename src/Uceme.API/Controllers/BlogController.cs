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

namespace Uceme.API.Controllers;

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
        var result = this.HandleControllerOperation(
            () => this.blogService.GetBlogSubset(amount),
            "retrieving blog subset with amount",
            amount);
        return this.Ok(result);
    }

    [HttpGet("getbloglist")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Blog>> GetBlogList(int page = 1)
    {
        var result = this.HandleControllerOperation(
            () => this.blogService.GetBlogSubset(page == 1 ? 10 : 12, page),
            "retrieving blog list for page",
            page);
        return this.Ok(result);
    }

    [HttpGet("getallposts")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Blog>> GetAllPosts()
    {
        var result = this.HandleControllerOperation(
            this.blogService.GetAllPosts,
            "retrieving all blog posts");
        return this.Ok(result);
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

        var result = this.HandleControllerOperation(
            () =>
            {
                var result = this.blogService.GetPost(slug);
                if (result == null)
                {
                    throw new Exception($"Blog post with slug '{slug}' not found");
                }

                return result;
            },
            "retrieving blog post with slug",
            slug);
        return this.Ok(result);
    }

    [HttpDelete("deletepost/{postId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> DeletePost(int postId)
    {
        var result = this.HandleControllerOperation(
            () =>
            {
                var result = this.blogService.DeletePost(postId);
                if (!result)
                {
                    throw new Exception($"Blog post with ID {postId} not found");
                }

                return result;
            },
            "deleting blog post",
            postId);
        return this.Ok(result);
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

        var result = this.HandleControllerOperation(
            () =>
            {
                var result = postRequest.IdBlog != 0 ?
                    this.blogService.UpdatePost(postRequest) :
                    this.blogService.AddPost(postRequest);
                return result;
            },
            $"{(postRequest.IdBlog != 0 ? "updating" : "adding")} blog post",
            postRequest.IdBlog);
        return this.Ok(result);
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
            string filename = "Blog" + Guid.NewGuid().ToString("N");
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

    private T HandleControllerOperation<T>(Func<T> operation, string errorContext, object? contextId = null)
    {
        try
        {
            return operation();
        }
        catch (DataException ex)
        {
            // Sanitize contextId to avoid log injection from user-provided values
            static string Sanitize(string input)
            {
                if (string.IsNullOrEmpty(input))
                {
                    return "null";
                }

                // Remove CR/LF, braces and other control characters to avoid log injection
                var cleaned = new string(input.Where(c => c != '\r' && c != '\n' && c != '{' && c != '}' && !char.IsControl(c)).ToArray());
                return string.IsNullOrEmpty(cleaned) ? "null" : cleaned;
            }

            string safeContextId = contextId switch
            {
                null => "null",
                string s => Sanitize(s),
                _ => Sanitize(contextId.ToString() ?? "null"),
            };

            // Truncate to a reasonable length to avoid excessively long log entries
            if (safeContextId.Length > 200)
            {
                safeContextId = string.Concat(safeContextId.AsSpan(0, 200), "...");
            }

            this.logger.LogError(ex, "Error {ErrorContext} - ContextId: {ContextId}", errorContext, safeContextId);
            throw new InvalidOperationException($"Unable to {errorContext.ToUpperInvariant()}", ex);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error {ErrorContext}", errorContext);
            throw new InvalidOperationException($"An unexpected error occurred while {errorContext.ToUpperInvariant()}", ex);
        }
    }
}
