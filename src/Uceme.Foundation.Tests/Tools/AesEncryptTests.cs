namespace Uceme.Foundation.Tests.Tools
{
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uceme.Foundation.Tools;

    [TestClass]
    public class AesEncryptTests
    {
        [TestMethod]
        [TestCategory("MockTests")]
        [Ignore("ski pre-season")]
        public void EncryptShouldEncryptWithoutIssues()
        {
            //// ARRRANGE
            string input = "hello world!";
            string appTest = "sybzoqdxaekulfjd";
            string appContact = "pjczzwbdpxmnfaet";

            string expected = "qL84V8anpKPDf8nRJMJ+fA==";
            string expected2 = "Rqm8TCkm8iB9uKj0oti+Uj7Y1IHPpVD9jZdummMgUgE=";
            string expected3 = "LTay33lhQzsXa7Vofa3H82ZEmzSXOghlRn6b4fw07gU=";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                expected = "xszTZjF/6XCxnIpfJqv7lQ==";
            }

            //// ACT
            string output = AesEncrypt.Encrypt(input);
            string output2 = AesEncrypt.Encrypt(appTest);
            string output3 = AesEncrypt.Encrypt(appContact);

            //// ASSERT
            Assert.AreEqual(expected, output);
            Assert.AreEqual(expected2, output2);
            Assert.AreEqual(expected3, output3);
        }
    }
}
