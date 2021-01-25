namespace Uceme.API.Services
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Uceme.API.Utilities;
    using Uceme.Model.Data;
    using Uceme.Model.Settings;

    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> logger;

        public EmailService(IOptions<AuthMessageSenderSettings> optionsAccessor, ILogger<EmailService> logger, ApplicationDbContext context, IEmailSender emailSender)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            this.logger = logger;
            this.DbContext = context;
            this.EmailSender = emailSender;
            this.Options = optionsAccessor.Value;
        }

        public ApplicationDbContext DbContext { get; }

        public IEmailSender EmailSender { get; }

        public AuthMessageSenderSettings Options { get; } //set only via Secret Manager

        public bool SendEmailToManagement(string fromAddress, string subject, string body)
        {
            try
            {
                this.EmailSender.SendEmailAsync(fromAddress, subject, body);
                return true;
            }
            catch (Exception e)
            {
                this.logger.LogError("error sending email to management");
                throw new OperationCanceledException("Error sending email to management", e);
            }
        }

        public bool SendEmailToClient(string toAddress, string subject, string body)
        {
            try
            {
                var toAddresses = new List<string>
                {
                    toAddress,
                    this.Options.EmailFrom,
                };
                
                this.EmailSender.SendEmailAsync(toAddresses, subject, body);
                return true;
            }
            catch (Exception e)
            {
                this.logger.LogError("error sending email to client");
                throw new OperationCanceledException("Error sending email to client", e);
            }
        }
    }
}
