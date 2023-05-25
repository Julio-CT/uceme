namespace Uceme.Library.Services;

using System.Threading.Tasks;

public interface IEmailService
{
    Task<bool> SendEmailToManagementAsync(string fromAddress, string subject, string body);

    bool SendEmailToManagement(string fromAddress, string subject, string body);

    Task<bool> SendEmailToClientAsync(string toAddress, string subject, string body);
}
