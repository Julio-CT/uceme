using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Uceme.Library.Services;
using Uceme.Model.DataContracts;

namespace Uceme.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ContactController : BaseController
{
    private readonly IEmailService emailService;

    public ContactController(
        ILogger<ContactController> logger,
        IEmailService emailService)
        : base(logger)
    {
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

        var result = await this.HandleControllerOperationAsync(
            async () => await this.emailService.SendEmailToManagementAsync(
                message.Email,
                $"Email recibido de {message.Name}",
                $"<p>Correo de contacto recibido de {message.Name}</p><p>{message.Message}</p>").ConfigureAwait(false),
            $"sending email from {message.Email} ({message.Name})",
            new { message.Email, message.Name }).ConfigureAwait(false);
        return this.Ok(result);
    }
}
