namespace Uceme.API.Services
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Logging;
    using Uceme.Model.Data;

    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> logger;

        public ApplicationDbContext DbContext { get; }

        public IEmailSender EmailSender { get; }

        public EmailService(ILogger<EmailService> logger, ApplicationDbContext context, IEmailSender emailSender)
        {
            this.logger = logger;
            this.DbContext = context;
            this.EmailSender = emailSender;
        }

        public bool SendEmail(string toAddress, string subject, string body)
        {
            try
            {
                this.EmailSender.SendEmailAsync(toAddress, subject, body);
                return true;
            }
            catch (Exception e)
            {
                throw new OperationCanceledException("Error sending email", e);
            }
        }
    }
}
