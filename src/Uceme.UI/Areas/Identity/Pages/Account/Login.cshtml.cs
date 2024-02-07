namespace Uceme.UI.Areas.Identity.Pages.Account;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

[AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
public class LoginModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
{
    private readonly SignInManager<Uceme.Model.Models.Security.ApplicationUser> signInManager;

    private readonly ILogger<LoginModel> logger;

    public LoginModel(
        SignInManager<Uceme.Model.Models.Security.ApplicationUser> signInManager,
        ILogger<LoginModel> logger)
    {
        this.signInManager = signInManager;
        this.logger = logger;
    }

    [BindProperty]
    public Uceme.Model.Models.Security.InputModel? Input { get; set; }

    public IList<AuthenticationScheme>? ExternalLogins { get; }

#pragma warning disable CA1056 // URI-like properties should not be strings
    public string? ReturnUrl { get; set; }
#pragma warning restore CA1056 // URI-like properties should not be strings

    [TempData]
    public string? ErrorMessage { get; set; }

#pragma warning disable CA1054 // URI-like parameters should not be strings
    public async Task OnGetAsync(string? returnUrl = null)
#pragma warning restore CA1054 // URI-like parameters should not be strings
    {
        if (!string.IsNullOrEmpty(this.ErrorMessage))
        {
            this.ModelState.AddModelError(string.Empty, this.ErrorMessage);
        }

        returnUrl ??= this.Url.Content("~/") ?? string.Empty;
        this.ReturnUrl = returnUrl;

        // Clear the existing external cookie to ensure a clean login process
        await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);

        if (this.ExternalLogins != null)
        {
            foreach (AuthenticationScheme? login in (await this.signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).ToList())
            {
                this.ExternalLogins.Add(login);
            }
        }
    }

#pragma warning disable CA1054 // URI-like parameters should not be strings
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
#pragma warning restore CA1054 // URI-like parameters should not be strings
    {
        returnUrl ??= this.Url.Content("~/") ?? string.Empty;

        if (this.ModelState.IsValid)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            Microsoft.AspNetCore.Identity.SignInResult result = await this.signInManager.PasswordSignInAsync(
                this.Input?.Email != null ? this.Input.Email : string.Empty,
                this.Input?.Password != null ? this.Input.Password : string.Empty,
                this.Input?.RememberMe != null && this.Input.RememberMe,
                lockoutOnFailure: false).ConfigureAwait(false);
            if (result.Succeeded)
            {
                this.logger.LogInformation("User logged in.");
                return this.LocalRedirect(returnUrl);
            }

            if (result.RequiresTwoFactor)
            {
                return this.RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = this.Input != null && this.Input.RememberMe });
            }

            if (result.IsLockedOut)
            {
                this.logger.LogWarning("User account locked out.");
                return this.RedirectToPage("./Lockout");
            }
            else
            {
                this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return this.Page();
            }
        }

        // If we got this far, something failed, redisplay form
        return this.Page();
    }
}
