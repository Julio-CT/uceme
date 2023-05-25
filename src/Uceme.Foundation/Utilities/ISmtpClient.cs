namespace Uceme.Foundation.Utilities;

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public interface ISmtpClient : IDisposable
{
    string? Host { get; set; }

    int Port { get; set; }

    SmtpDeliveryMethod DeliveryMethod { get; set; }

    bool UseDefaultCredentials { get; set; }

    ICredentialsByHost? Credentials { get; set; }

    bool EnableSsl { get; set; }

    void Send(MailMessage mailMessage);

    Task SendMailAsync(MailMessage mailMessage);
}
