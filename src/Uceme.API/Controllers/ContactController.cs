namespace Uceme.API.Controllers;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Uceme.Library.Services;
using Uceme.Model.DataContracts;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ContactController : Controller
{
    private readonly ILogger<ContactController> logger;
    private readonly IEmailService emailService;

    public ContactController(
        ILogger<ContactController> logger,
        IEmailService emailService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    [HttpPost("contactemail")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> ContactEmailAsync([FromBody] EmailMessage message)
    {
        if (message is null || string.IsNullOrEmpty(message.Email))
        {
            return this.BadRequest(new ProblemDetails
            {
                Title = "Invalid Request",
                Detail = "Email message and email address are required",
                Status = StatusCodes.Status400BadRequest,
            });
        }

        try
        {
            string body = $"<p>Correo de contacto recibido de {message.Name}</p><p>{message.Message}</p>";
            var result = await this.emailService.SendEmailToManagementAsync(
                message.Email,
                $"Email recibido de {message.Name}",
                body).ConfigureAwait(false);
            return this.Ok(result);
        }
        catch (OperationCanceledException ex)
        {
            this.logger.LogError(ex, "Error sending email from {Email} ({Name})", message.Email, message.Name);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Email Service Error",
                Detail = "Unable to send email due to a timeout",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unexpected error sending email from {Email} ({Name})", message.Email, message.Name);
            return this.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Server Error",
                Detail = "An unexpected error occurred while sending the email",
                Status = StatusCodes.Status500InternalServerError,
            });
        }
    }
}
