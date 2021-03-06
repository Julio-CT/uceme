﻿namespace Uceme.API.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Uceme.Foundation.Utilities;
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
            this.dbContext = context;
            this.EmailSender = emailSender;
            this.Options = optionsAccessor.Value;
        }

        private readonly ApplicationDbContext dbContext;

        public IEmailSender EmailSender { get; }

        public AuthMessageSenderSettings Options { get; } //set only via Secret Manager

        public async Task<bool> SendEmailToManagementAsync(string fromAddress, string subject, string body)
        {
            try
            {
                var toAddresses = new List<string>()
                {
                    this.Options.EmailFrom,
                };

                if (!string.IsNullOrEmpty(fromAddress))
                {
                    toAddresses.Add(fromAddress);
                }

                await this.EmailSender.SendEmailAsync(toAddresses, subject, body).ConfigureAwait(false);
                return true;
            }
            catch (Exception e)
            {
                this.logger.LogError("error sending email to management");
                throw new OperationCanceledException("Error sending email to management", e);
            }
        }

        public bool SendEmailToManagement(string fromAddress, string subject, string body)
        {
            try
            {
                var addresses = new List<string>()
                {
                    this.Options.EmailFrom,
                };

                if (!string.IsNullOrEmpty(fromAddress))
                {
                    addresses.Add(fromAddress);
                }

                this.EmailSender.SendEmail(fromAddress, subject, body);
                return true;
            }
            catch (Exception e)
            {
                this.logger.LogError("error sending email to management");
                throw new OperationCanceledException("Error sending email to management", e);
            }
        }

        public async Task<bool> SendEmailToClientAsync(string toAddress, string subject, string body)
        {
            try
            {
                var toAddresses = new List<string>
                {
                    toAddress,
                    this.Options.EmailFrom,
                };

                await EmailSender.SendEmailAsync(toAddresses, subject, body).ConfigureAwait(false);
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
