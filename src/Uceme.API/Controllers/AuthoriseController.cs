namespace Uceme.API.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Uceme.Model.Data;
    using Uceme.Model.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthoriseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly ILogger<AuthoriseController> logger;

        public AuthoriseController(
            ILogger<AuthoriseController> logger,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public async Task<Microsoft.AspNetCore.Identity.SignInResult> Login(string email, string password, bool rememberMe)
        {
            return await this.signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false).ConfigureAwait(false);
        }

        [HttpGet("Logout")]
        [AllowAnonymous]
        public async Task Logout()
        {
            await this.signInManager.SignOutAsync().ConfigureAwait(false);
            this.logger.LogInformation("User logged out.");
        }

        [HttpGet("Register")]
        [AllowAnonymous]
        public async Task<IdentityResult> Register(string email, string password)
        {
            var user = new ApplicationUser { UserName = email, Email = email };
            return await this.userManager.CreateAsync(user, password).ConfigureAwait(false);
        }
    }
}
