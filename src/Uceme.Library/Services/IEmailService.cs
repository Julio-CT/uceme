using System.Threading.Tasks;

namespace Uceme.Library.Services;

public interface IEmailService
{
    Task<bool> SendEmailToManagementAsync(string fromAddress, string subject, string body);

    Task<bool> SendEmailToClientAsync(string toAddress, string subject, string body);
}
