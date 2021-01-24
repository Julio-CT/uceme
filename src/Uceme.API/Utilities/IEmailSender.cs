namespace Uceme.API.Utilities
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Uceme.Model.Settings;

    public interface IEmailSender
    {
        AuthMessageSenderSettings Options { get; }

        Task SendEmailAsync(string email, string subject, string htmlMessage);

        Task SendEmailAsync(IEnumerable<string> emails, string subject, string htmlMessage);
    }
}