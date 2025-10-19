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

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> logger;
    private readonly IEmailSender emailSender;

    public EmailService(
        IOptions<AuthMessageSenderSettings> optionsAccessor,
        ILogger<EmailService> logger,
        IEmailSender emailSender)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        this.Options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
    }

    public AuthMessageSenderSettings Options { get; } // set only via Secret Manager

    public async Task<bool> SendEmailToManagementAsync(string fromAddress, string subject, string body)
    {
        this.ValidateEmailConfig();
        ValidateBody(body);
        ValidateSubject(subject);
        if (!string.IsNullOrEmpty(fromAddress))
        {
            ValidateEmailAddress(fromAddress);
        }

        var toAddresses = this.BuildManagementRecipientList(fromAddress);
        return await this.SendEmailInternalAsync(toAddresses, subject, body, "management").ConfigureAwait(false);
    }

    public async Task<bool> SendEmailToClientAsync(string toAddress, string subject, string body)
    {
        this.ValidateEmailConfig();
        ValidateBody(body);
        ValidateSubject(subject);
        ValidateEmailAddress(toAddress);

        var toAddresses = this.BuildClientRecipientList(toAddress);
        return await this.SendEmailInternalAsync(toAddresses, subject, body, "client").ConfigureAwait(false);
    }

    private static void ValidateBody(string body)
    {
        if (string.IsNullOrEmpty(body) || (!string.IsNullOrEmpty(body) && Regex.IsMatch(body, @"^[ ]+$")))
        {
            throw new ArgumentException("the body provided is not valid");
        }
    }

    private static void ValidateSubject(string subject)
    {
        if (string.IsNullOrEmpty(subject) || (!string.IsNullOrEmpty(subject) && !Regex.IsMatch(subject, @"^[a-zA-Z0-9_\. ]+$")))
        {
            throw new ArgumentException("the subject provided is not valid");
        }
    }

    private static void ValidateEmailAddress(string email)
    {
        if (string.IsNullOrEmpty(email) || !MailAddress.TryCreate(email, out MailAddress? _))
        {
            throw new ArgumentException("the email address provided is not valid");
        }
    }

    private void ValidateEmailConfig()
    {
        if (this.Options.EmailFrom == null)
        {
            throw new MissingFieldException(nameof(this.Options.EmailFrom));
        }
    }

    private List<string> BuildManagementRecipientList(string fromAddress)
    {
        var toAddresses = new List<string>();
        if (!string.IsNullOrEmpty(this.Options.EmailFrom))
        {
            toAddresses.Add(this.Options.EmailFrom);
        }

        if (!string.IsNullOrEmpty(fromAddress))
        {
            toAddresses.Add(fromAddress);
        }

        return toAddresses;
    }

    private List<string> BuildClientRecipientList(string toAddress)
    {
        var toAddresses = new List<string> { toAddress };
        if (!string.IsNullOrEmpty(this.Options.EmailFrom))
        {
            toAddresses.Add(this.Options.EmailFrom);
        }

        return toAddresses;
    }

    private async Task<bool> SendEmailInternalAsync(List<string> toAddresses, string subject, string body, string context)
    {
        try
        {
            await this.emailSender.SendEmailAsync(toAddresses, subject, body).ConfigureAwait(false);
            return true;
        }
        catch (Exception e)
        {
            this.logger.LogError($"Error sending email to {context}: {{EMessage}}", e.Message);
            throw new OperationCanceledException($"Error sending email to {context}", e);
        }
    }
}
