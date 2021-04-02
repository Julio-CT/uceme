namespace Uceme.API.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Uceme.Library.Services;
    using Uceme.Model.DataContracts;

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
            this.logger = logger;
            this.emailService = emailService;
        }

        [HttpPost("contactemail")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> ContactEmailAsync([FromBody] EmailMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            bool result = false;
            try
            {
                var body = $"<p>Correo de contacto recibido de {message.Name}</p><p>{message.Message}</p>";
                result = await this.emailService.SendEmailToManagementAsync(message.Email, $"Email recibido de {message.Name}", body).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                this.logger.LogError("error sending email");
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return result;
        }
    }
}
