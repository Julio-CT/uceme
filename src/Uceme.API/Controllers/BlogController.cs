namespace Uceme.API.Controllers
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Uceme.API.Services;
    using Uceme.Model.Models;

    [Route("api/[controller]")]
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

        [HttpGet("getblogsubset")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Blog>> GetBlogSubset(int amount)
        {
            IEnumerable<Blog> result;
            try
            {
                result = this.blogService.GetBlogSubset(amount);
            }
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result.ToList();
        }

        [HttpGet("getbloglist")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Blog>> GetBlogList(int page)
        {
            IEnumerable<Blog> result;
            try
            {
                result = this.blogService.GetBlogSubset(page == 1 ? 10 : 12, page);
            }
            catch (DataException)
            {
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
            catch (DataException)
            {
                return this.BadRequest();
            }

            return result;
        }
    }
}