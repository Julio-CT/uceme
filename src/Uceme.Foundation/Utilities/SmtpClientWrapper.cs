namespace Uceme.Foundation.Utilities
{
    using System;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class SmtpClientWrapper : SmtpClient, ISmtpClient
    {
        private bool disposed;

        ~SmtpClientWrapper()
        {
            Dispose(false);
        }

        public new void Send(MailMessage mailMessage)
        {
            CheckDisposed();
            base.Send(mailMessage);
        }

        public new async Task SendMailAsync(MailMessage mailMessage)
        {
            CheckDisposed();
            await base.SendMailAsync(mailMessage).ConfigureAwait(false);
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected new virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
            }
        }

        protected void CheckDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(SmtpClientWrapper));
            }
        }
    }
}
