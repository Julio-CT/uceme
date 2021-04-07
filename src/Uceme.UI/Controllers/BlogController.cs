namespace Uceme.UI.Controllers
{
    using System.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Uceme.Library.Services;
    using Uceme.Model.Models;

    [Route("clientapi/[controller]")]
    [ApiController]
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> logger;

        private readonly IBlogService blogService;

        public BlogController(IBlogService blogService, ILogger<BlogController> logger)
        {
            this.blogService = blogService;
            this.logger = logger;
        }

        [HttpGet("deletepost")]
        public ActionResult<bool> DeletePost(int postId)
        {
            bool result = false;
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
            Blog result = null;
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
    }
}
