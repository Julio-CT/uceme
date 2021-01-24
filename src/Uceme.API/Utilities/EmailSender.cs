namespace Uceme.API.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Uceme.Model.Settings;

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
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrEmpty(htmlMessage))
            {
                throw new ArgumentNullException(nameof(htmlMessage));
            }

            return this.Execute(subject, htmlMessage, new List<string> { email });
        }

        public Task SendEmailAsync(IEnumerable<string> emails, string subject, string htmlMessage)
        {
            if (emails == null)
            {
                throw new ArgumentNullException(nameof(emails));
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrEmpty(htmlMessage))
            {
                throw new ArgumentNullException(nameof(htmlMessage));
            }

            return this.Execute(subject, htmlMessage, emails);
        }

        private Task Execute(string subject, string message, IEnumerable<string> emails)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(this.Options.EmailFrom, "From Name");
                foreach (var email in emails)
                {
                    mailMessage.To.Add(new MailAddress(email, "To Name"));
                }

                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;

                using (var smtpServer = new SmtpClient())
                {
                    smtpServer.Host = this.Options.HostSmtp;
                    smtpServer.Port = this.Options.PortSmtp;

                    //hasta configurar la cuenta de envio, supongo que con las credenciales de esta, será valida
                    smtpServer.Credentials = new NetworkCredential(this.Options.CredentialUser, this.Options.CredentialPassword);
                    smtpServer.EnableSsl = true;

                    smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpServer.UseDefaultCredentials = false;
                    return smtpServer.SendMailAsync(mailMessage);
                }
            }
        }
    }
}
