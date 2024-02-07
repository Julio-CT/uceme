namespace Uceme.Foundation.Utilities;

using System;
using System.Net.Mail;
using System.Threading.Tasks;

public class SmtpClientWrapper : SmtpClient, ISmtpClient
{
    private bool disposed;

    ~SmtpClientWrapper()
    {
        this.Dispose(false);
    }

    public new void Send(MailMessage mailMessage)
    {
        this.CheckDisposed();
        base.Send(mailMessage);
    }

    public new async Task SendMailAsync(MailMessage mailMessage)
    {
        this.CheckDisposed();
        await base.SendMailAsync(mailMessage).ConfigureAwait(false);
    }

    public new void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected new virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            this.disposed = true;
        }
    }

    private void CheckDisposed()
    {
        if (this.disposed)
        {
            throw new ObjectDisposedException(nameof(SmtpClientWrapper));
        }
    }
}
