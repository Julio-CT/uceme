namespace Uceme.API.Controllers
{
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
    using SixLabors.ImageSharp.Formats.Webp;
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
            this.blogService = blogService;
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpGet("getblogsubset")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Blog>> GetBlogSubset(int amount)
        {
            IEnumerable<Blog> result;
            try
            {
                result = this.blogService.GetBlogSubset(amount);
            }
            catch (DataException ex)
            {
                this.logger.LogError(ex.Message);
                return this.BadRequest();
            }

            return result.ToList();
        }

        [HttpGet("getbloglist")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Blog>> GetBlogList(int page = 1)
        {
            IEnumerable<Blog> result;
            try
            {
                result = this.blogService.GetBlogSubset(page == 1 ? 10 : 12, page);
            }
            catch (DataException ex)
            {
                this.logger.LogError(ex.Message);
                return this.BadRequest();
            }

            return result.ToList();
        }

        [HttpGet("getallposts")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Blog>> GetAllPosts()
        {
            IEnumerable<Blog> result;
            try
            {
                result = this.blogService.GetAllPosts();
            }
            catch (DataException ex)
            {
                this.logger.LogError(ex.Message);
                return this.BadRequest();
            }

            return result.ToList();
        }

        [HttpGet("getpost")]
        [AllowAnonymous]
        public ActionResult<Blog> GetPost(string slug)
        {
            Blog result;
            try
            {
                result = this.blogService.GetPost(slug);
            }
            catch (DataException ex)
            {
                this.logger.LogError(ex.Message);
                return this.BadRequest();
            }

            return result;
        }

        [HttpGet("deletepost")]
        public ActionResult<bool> DeletePost(int postId)
        {
            bool result;
            try
            {
                result = this.blogService.DeletePost(postId);
            }
            catch (DataException ex)
            {
                this.logger.LogError(ex.Message);
                return this.BadRequest();
            }

            return result;
        }

        [HttpGet("updatepost")]
        public ActionResult<Blog> UpdatePost(Blog post)
        {
            if (post == null)
            {
                return this.BadRequest("post is null");
            }

            Blog result;
            try
            {
                result = this.blogService.UpdatePost(post);
            }
            catch (DataException ex)
            {
                this.logger.LogError(ex.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return result;
        }

        [HttpPost("addpost")]
        public ActionResult<bool> AddPost([FromBody] PostRequest postRequest)
        {
            if (postRequest == null)
            {
                return this.BadRequest("post is null");
            }

            bool result;
            try
            {
                if (postRequest.IdBlog != 0)
                {
                    result = this.blogService.UpdatePost(postRequest);
                }
                else
                {
                    result = this.blogService.AddPost(postRequest);
                }
            }
            catch (DataException ex)
            {
                this.logger.LogError(ex.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return result;
        }

        [HttpPost("onpostuploadasync")]
        public async Task<ActionResult<string>> OnPostUploadAsync([FromForm] IFormFile file)
        {
            if (file is null)
            {
                return this.BadRequest("file to upload is null");
            }

            if (this.configuration?.Value?.BlogImagesDir == null
                || !Directory.Exists(this.configuration?.Value?.BlogImagesDir))
            {
                return this.StatusCode(StatusCodes.Status502BadGateway);
            }

            string[] allowedImageTypes = new string[] { "IMAGE/JPEG", "IMAGE/PNG" };
            if (!allowedImageTypes.Contains(file.ContentType.ToUpperInvariant()))
            {
                return this.BadRequest("file to upload is not in the correct format");
            }

            string webPFileName = string.Empty;

            try
            {
                if (file.Length > 0 && Path.GetExtension(file.FileName).Length < 6)
                {
                    string filename = "Blog" + this.blogService.GetNextPostImage();
                    webPFileName = filename + ".webp";

                    filename += Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(
                        this.configuration.Value.BlogImagesDir,
                        filename);
                    string webPImagePath = Path.Combine(webPFileName, webPFileName);

#pragma warning disable CA3003 // Review code for file path injection vulnerabilities
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        //// Save the image in its original format for fallback
                        await file.CopyToAsync(stream).ConfigureAwait(false);

                        // Then save in WebP format
                        using (var someImage = await Image.LoadAsync(stream).ConfigureAwait(false))
                        {
                            using (var outStream = new MemoryStream())
                            {
                                await someImage.SaveAsync(outStream, new WebpEncoder()).ConfigureAwait(false);

                                return new FileContentResult(outStream.ToArray(), "image/webp");
                            }
                        }
                    }
#pragma warning restore CA3003 // Review code for file path injection vulnerabilities
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return webPFileName;
        }
    }
}
