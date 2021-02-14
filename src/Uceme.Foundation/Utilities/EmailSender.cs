namespace Uceme.Foundation.Utilities
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

        public EmailSender(IOptions<AuthMessageSenderSettings> optionsAccessor, ISmtpClient smtpClient)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            this.Options = optionsAccessor.Value;
            this.SmtpClient = smtpClient;
        }

        public AuthMessageSenderSettings Options { get; } //set only via Secret Manager

        public ISmtpClient SmtpClient { get; } //set only via Secret Manager

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            if (string.IsNullOrEmpty(toEmail))
            {
                throw new ArgumentNullException(nameof(toEmail));
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrEmpty(htmlMessage))
            {
                throw new ArgumentNullException(nameof(htmlMessage));
            }

            await ExecuteAsync(subject, htmlMessage, new List<string> { toEmail }).ConfigureAwait(false);
        }

        public async Task SendEmailAsync(IEnumerable<string> toEmails, string subject, string htmlMessage)
        {
            if (toEmails == null)
            {
                throw new ArgumentNullException(nameof(toEmails));
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrEmpty(htmlMessage))
            {
                throw new ArgumentNullException(nameof(htmlMessage));
            }

            await this.ExecuteAsync(subject, htmlMessage, toEmails).ConfigureAwait(false);
        }

        public void SendEmail(string toEmail, string subject, string htmlMessage)
        {
            if (string.IsNullOrEmpty(toEmail))
            {
                throw new ArgumentNullException(nameof(toEmail));
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrEmpty(htmlMessage))
            {
                throw new ArgumentNullException(nameof(htmlMessage));
            }

            this.Execute(subject, htmlMessage, new List<string> { toEmail });
        }

        public void SendEmail(IEnumerable<string> toEmails, string subject, string htmlMessage)
        {
            if (toEmails == null)
            {
                throw new ArgumentNullException(nameof(toEmails));
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrEmpty(htmlMessage))
            {
                throw new ArgumentNullException(nameof(htmlMessage));
            }

            this.Execute(subject, htmlMessage, toEmails);
        }

        private async Task ExecuteAsync(string subject, string message, IEnumerable<string> toEmails)
        {
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
