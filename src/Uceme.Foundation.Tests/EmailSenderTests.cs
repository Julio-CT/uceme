namespace Uceme.Foundation.Tests
{
    using System.Threading.Tasks;
    using AutoMoq;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uceme.Foundation.Utilities;
    using Uceme.Model.Settings;

    [TestClass]
    public class EmailSenderTests
    {
        private readonly string emailTo = "julio_cejudo@yahoo.com";

        [TestMethod()]
        public async Task SendPasswordRetrievalTestAsync()
        {
            //// ARRANGE
            var mocker = new AutoMoqer();
            var moqOptions = mocker.Create<IOptions<AuthMessageSenderSettings>>();
            var sut = new EmailSender(moqOptions);
            var email = this.emailTo;
            var subject = "BB";
            var htmlMessage = "CC";

            /// ACT
            await sut.SendEmailAsync(email, subject, htmlMessage).ConfigureAwait(false);

            //// ASSERT
        }
    }
}
