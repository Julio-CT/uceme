namespace Uceme.API.Services
{
    using Uceme.Model.Data;

    public interface IEmailService
    {
        ApplicationDbContext DbContext { get; }

        bool SendEmailToManagement(string fromAddress, string subject, string body);

        bool SendEmailToClient(string toAddress, string subject, string body);
    }
}