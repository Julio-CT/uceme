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

    [ApiController]
    [Route("[controller]")]
    public class BlogController : Controller
    {
        private readonly ILogger<HomeController> logger;
    
        private readonly IBlogService blogService;

        public BlogController(IBlogService blogService, ILogger<HomeController> logger)
        {
            this.blogService = blogService;
            this.logger = logger;
        }

        [HttpGet("[controller]/getblogsubset")]
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