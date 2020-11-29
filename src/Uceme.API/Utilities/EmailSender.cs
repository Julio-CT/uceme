﻿namespace Uceme.API.Utilities
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;
    using Uceme.API.Options;

    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderSettings> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            this.Options = optionsAccessor.Value;
        }

        public AuthMessageSenderSettings Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(this.Options, subject, htmlMessage, email);
        }

        private static Task Execute(AuthMessageSenderSettings apiKey, string subject, string message, string email)
        {
            if (apiKey == null)
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(apiKey.EmailFrom);
                mailMessage.To.Add(new MailAddress(email));
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;

                using (var smtpServer = new SmtpClient())
                {
                    smtpServer.Host = apiKey.HostSmtp;

                    smtpServer.Port = apiKey.PortSmtp;

                    //hasta configurar la cuenta de envio, supongo que con las credenciales de esta, será valida
                    smtpServer.Credentials = new NetworkCredential(apiKey.CredentialUser, apiKey.CredentialPassword);
                    smtpServer.EnableSsl = true;
                    return smtpServer.SendMailAsync(mailMessage);
                }
            }
        }
    }
}