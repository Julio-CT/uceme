namespace Uceme.UI.Areas.Identity.Pages.Account
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using Uceme.Model.Models;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class LogoutModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly ILogger<LogoutModel> logger;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger)
        {
            this.signInManager = signInManager;
            this.logger = logger;
        }

        public void OnGet()
        {
        }

#pragma warning disable CA1054 // URI-like parameters should not be strings
        public async Task<IActionResult> OnPost(string? returnUrl = null)
#pragma warning restore CA1054 // URI-like parameters should not be strings
        {
            await this.signInManager.SignOutAsync().ConfigureAwait(false);
            this.logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return this.LocalRedirect(returnUrl);
            }
            else
            {
                return this.RedirectToPage();
            }
        }
    }
}
