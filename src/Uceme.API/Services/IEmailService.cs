namespace Uceme.API.Services
{
    using Uceme.Model.Data;

    public interface IEmailService
    {
        ApplicationDbContext DbContext { get; }

        bool SendEmail(string toAddress, string subject, string body);
    }
}