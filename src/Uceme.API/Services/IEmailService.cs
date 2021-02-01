namespace Uceme.API.Services
{
    using System.Threading.Tasks;
    using Uceme.Model.Data;

    public interface IEmailService
    {
        ApplicationDbContext DbContext { get; }

        Task<bool> SendEmailToManagementAsync(string fromAddress, string subject, string body);

        bool SendEmailToManagement(string fromAddress, string subject, string body);

        Task<bool> SendEmailToClientAsync(string toAddress, string subject, string body);
    }
}