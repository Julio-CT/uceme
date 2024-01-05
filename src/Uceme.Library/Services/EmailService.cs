namespace Uceme.Library.Services;

using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Uceme.Foundation.Utilities;
using Uceme.Model.Settings;
using static IdentityServer4.Models.IdentityResources;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> logger;

    public EmailService(
        IOptions<AuthMessageSenderSettings> optionsAccessor,
        ILogger<EmailService> logger,
        IEmailSender emailSender)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.EmailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        this.Options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
    }

    public IEmailSender EmailSender { get; }

    public AuthMessageSenderSettings Options { get; } // set only via Secret Manager

    public async Task<bool> SendEmailToManagementAsync(string fromAddress, string subject, string body)
    {
        if (this.Options.EmailFrom == null)
        {
            throw new MissingFieldException(nameof(this.Options.EmailFrom));
        }

        if (string.IsNullOrEmpty(body) || (!string.IsNullOrEmpty(body) && Regex.IsMatch(body, @"^[ ]+$")))
        {
            throw new ArgumentException("the body provided is not valid");
        }

        if (string.IsNullOrEmpty(subject) || (!string.IsNullOrEmpty(subject) && !Regex.IsMatch(subject, @"^[a-zA-Z0-9_\. ]+$")))
        {
            throw new ArgumentException("the subject  provided is not valid");
        }

        if (!string.IsNullOrEmpty(fromAddress) && !MailAddress.TryCreate(fromAddress, out var _))
        {
            throw new ArgumentException("the email address provided is not valid");
        }

        try
        {
            List<string> toAddresses = new List<string>()
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
            this.logger.LogError("error sending email to management:" + e.Message);
            throw new OperationCanceledException("Error sending email to management", e);
        }
    }

    public bool SendEmailToManagement(string fromAddress, string subject, string body)
    {
        if (this.Options.EmailFrom == null)
        {
            throw new MissingFieldException(nameof(this.Options.EmailFrom));
        }

        if (string.IsNullOrEmpty(body) || (!string.IsNullOrEmpty(body) && Regex.IsMatch(body, @"^[ ]+$")))
        {
            throw new ArgumentException("the body provided is not valid");
        }

        if (string.IsNullOrEmpty(subject) || (!string.IsNullOrEmpty(subject) && !Regex.IsMatch(subject, @"^[a-zA-Z0-9_\. ]+$")))
        {
            throw new ArgumentException("the subject provided is not valid");
        }

        if (!string.IsNullOrEmpty(fromAddress) && !MailAddress.TryCreate(fromAddress, out var _))
        {
            throw new ArgumentException("the email address provided is not valid");
        }

        try
        {
            List<string> toAddresses = new List<string>()
            {
                this.Options.EmailFrom,
            };

            if (!string.IsNullOrEmpty(fromAddress))
            {
                toAddresses.Add(fromAddress);
            }

            this.EmailSender.SendEmail(toAddresses, subject, body);
            return true;
        }
        catch (Exception e)
        {
            this.logger.LogError("error sending email to management:" + e.Message);
            throw new OperationCanceledException("Error sending email to management", e);
        }
    }

    public async Task<bool> SendEmailToClientAsync(string toAddress, string subject, string body)
    {
        if (this.Options.EmailFrom == null)
        {
            throw new MissingFieldException(nameof(this.Options.EmailFrom));
        }

        if (string.IsNullOrEmpty(body) || (!string.IsNullOrEmpty(body) && Regex.IsMatch(body, @"^[ ]+$")))
        {
            throw new ArgumentException("the body provided is not valid");
        }

        if (string.IsNullOrEmpty(subject) || (!string.IsNullOrEmpty(subject) && !Regex.IsMatch(subject, @"^[a-zA-Z0-9_\. ]+$")))
        {
            throw new ArgumentException("the subject provided is not valid");
        }

        if (string.IsNullOrEmpty(toAddress) || (!string.IsNullOrEmpty(toAddress) && !MailAddress.TryCreate(toAddress, out var _)))
        {
            throw new ArgumentException("the email address provided is not valid");
        }

        try
        {
            List<string> toAddresses = new List<string>
            {
                toAddress,
                this.Options.EmailFrom,
            };

            await this.EmailSender.SendEmailAsync(toAddresses, subject, body).ConfigureAwait(false);
            return true;
        }
        catch (Exception e)
        {
            this.logger.LogError("error sending email to client:" + e.Message);
            throw new OperationCanceledException("Error sending email to client", e);
        }
    }
}
