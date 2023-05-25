namespace Uceme.Foundation.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Uceme.Model.Settings;

    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderSettings> optionsAccessor, ISmtpClient smtpClient)
        {
            if (optionsAccessor == null || optionsAccessor.Value == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            this.Options = optionsAccessor.Value;
            this.SmtpClient = smtpClient;
        }

        public AuthMessageSenderSettings Options { get; } // set only via Secret Manager

        public ISmtpClient SmtpClient { get; } // set only via Secret Manager

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
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

            await this.ExecuteAsync(subject, htmlMessage, new List<string> { email }).ConfigureAwait(false);
        }

        public async Task SendEmailAsync(IEnumerable<string> emails, string subject, string htmlMessage)
        {
            if (emails == null || !emails.Any())
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

            await this.ExecuteAsync(subject, htmlMessage, emails).ConfigureAwait(false);
        }

        public void SendEmail(string email, string subject, string htmlMessage)
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

            this.Execute(subject, htmlMessage, new List<string> { email });
        }

        public void SendEmail(IEnumerable<string> emails, string subject, string htmlMessage)
        {
            if (emails == null || !emails.Any())
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

            this.Execute(subject, htmlMessage, emails);
        }

        private async Task ExecuteAsync(string subject, string message, IEnumerable<string> toEmails)
        {
            if (this.Options.EmailFrom == null)
            {
                throw new MissingFieldException(nameof(this.Options.EmailFrom));
            }

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(this.Options.EmailFrom, "Notificaciones UCEME");
                foreach (var email in toEmails)
                {
                    mailMessage.To.Add(new MailAddress(email, email));
                }

                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;

                using (var smtpServer = this.SmtpClient ?? new SmtpClientWrapper())
                {
                    smtpServer.Host = this.Options.HostSmtp;
                    smtpServer.Port = this.Options.PortSmtp;
                    smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpServer.UseDefaultCredentials = false;

                    smtpServer.Credentials = new NetworkCredential(this.Options.CredentialUser, this.Options.CredentialPassword);
                    smtpServer.EnableSsl = true;
                    try
                    {
                        await smtpServer.SendMailAsync(mailMessage).ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException("error sending email", e);
                    }
                }
            }
        }

        private void Execute(string subject, string message, IEnumerable<string> toEmails)
        {
            if (this.Options.EmailFrom == null)
            {
                throw new MissingFieldException(nameof(this.Options.EmailFrom));
            }

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(this.Options.EmailFrom, "From Name");
                foreach (var email in toEmails)
                {
                    mailMessage.To.Add(new MailAddress(email, "To Name"));
                }

                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;

                using (var smtpServer = this.SmtpClient ?? new SmtpClientWrapper())
                {
                    smtpServer.Host = this.Options.HostSmtp;
                    smtpServer.Port = this.Options.PortSmtp;
                    smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpServer.UseDefaultCredentials = false;

                    smtpServer.Credentials = new NetworkCredential(this.Options.CredentialUser, this.Options.CredentialPassword);
                    smtpServer.EnableSsl = true;
                    try
                    {
                        smtpServer.Send(mailMessage);
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException("error sending email", e);
                    }
                }
            }
        }
    }
}
