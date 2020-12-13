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
    }
}