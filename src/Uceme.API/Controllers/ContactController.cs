namespace Uceme.API.Controllers
{
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Uceme.API.Services;
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
        public ActionResult<bool> ContactEmail([FromBody] EmailMessage message)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            bool result;
            try
            {
                result = this.emailService.SendEmailToManagement(message.Name, message.Email, message.Message);
            }
            catch (OperationCanceledException)
            {
                this.logger.LogError("error sending email");
                return this.BadRequest();
            }

            return result;
        }
    }
}