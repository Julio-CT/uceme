namespace Uceme.UI.Controllers
{
    using System;
    using System.Data;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Uceme.Library.Services;
    using Uceme.Model.DataContracts;
    using Uceme.Model.Models;
    using Uceme.Model.Settings;

    [Route("clientapi/[controller]")]
    [ApiController]
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> logger;

        private readonly IBlogService blogService;

        private readonly IOptions<AppSettings> configuration;

        public BlogController(
            IBlogService blogService,
            IOptions<AppSettings> configuration,
            ILogger<BlogController> logger)
        {
            this.blogService = blogService;
            this.logger = logger;
            this.configuration = configuration;
        }

        [HttpGet("deletepost")]
        public ActionResult<bool> DeletePost(int postId)
        {
            bool result;
            try
            {
                result = this.blogService.DeletePost(postId);
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result;
        }

        [HttpGet("updatepost")]
        public ActionResult<Blog> UpdatePost(Blog post)
        {
            Blog result;
            try
            {
                result = this.blogService.UpdatePost(post);
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result;
        }

        [HttpPost("addpost")]
        public ActionResult<bool> AddPost([FromBody] PostRequest postRequest)
        {
            if (postRequest == null)
            {
                return this.BadRequest();
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
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result;
        }

        [HttpPost("onpostuploadasync")]
        public async Task<ActionResult<string>> OnPostUploadAsync([FromForm] IFormFile file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            string filename = null;

            try
            {
                if (file.Length > 0)
                {
                    filename = "Blog" + this.blogService.GetNextPostImage() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(
                        this.configuration.Value.BlogImagesDir,
                        filename);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception)
            {
                return this.BadRequest();
            }

            return filename;
        }
    }
}
