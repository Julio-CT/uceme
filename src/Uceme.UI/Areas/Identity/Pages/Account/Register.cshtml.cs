namespace Uceme.UI.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using Uceme.Model.Models;

    [AllowAnonymous]
#pragma warning disable SA1649 // File name should match first type name
    public class RegisterModel : PageModel
#pragma warning restore SA1649 // File name should match first type name
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public RegisterInputModel? Input { get; set; }

#pragma warning disable CA1056 // URI-like properties should not be strings
        public string? ReturnUrl { get; set; }
#pragma warning restore CA1056 // URI-like properties should not be strings

        public IList<AuthenticationScheme>? ExternalLogins { get; }

#pragma warning disable CA1054 // URI-like parameters should not be strings
        public async Task OnGetAsync(string? returnUrl = null)
#pragma warning restore CA1054 // URI-like parameters should not be strings
        {
            this.ReturnUrl = returnUrl;
            if (this.ExternalLogins != null)
            {
                foreach (var login in (await this.signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).ToList())
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
            if (this.ExternalLogins != null)
            {
                foreach (var login in (await this.signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).ToList())
                {
                    this.ExternalLogins.Add(login);
                }
            }

            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = this.Input?.Email, Email = this.Input?.Email };
                var result = await this.userManager.CreateAsync(user, this.Input?.Password).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    this.logger.LogInformation("User created a new account with password.");

                    var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = this.Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: this.Request.Scheme);

                    await this.emailSender.SendEmailAsync(
                        this.Input?.Email,
                        "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl ?? string.Empty)}'>clicking here</a>.").ConfigureAwait(false);

                    if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input?.Email });
                    }
                    else
                    {
                        await this.signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
