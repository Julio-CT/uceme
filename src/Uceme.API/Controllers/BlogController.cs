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
                return this.BadRequest("file upload is null");
            }

            if (this.configuration?.Value?.BlogImagesDir == null
                || !Directory.Exists(this.configuration?.Value?.BlogImagesDir))
            {
                return this.StatusCode(StatusCodes.Status502BadGateway);
            }

            string filename = string.Empty;

            try
            {
                if (file.Length > 0 && Path.GetExtension(file.FileName).Length < 6)
                {
                    filename = "Blog" + this.blogService.GetNextPostImage() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(
                        this.configuration.Value.BlogImagesDir,
                        filename);

#pragma warning disable CA3003 // Review code for file path injection vulnerabilities
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream).ConfigureAwait(false);
                    }
#pragma warning restore CA3003 // Review code for file path injection vulnerabilities
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return filename;
        }
    }
}
