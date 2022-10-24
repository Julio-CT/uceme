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
        public void EncryptShouldEncryptWithoutIssues()
        {
            //// ARRRANGE
            string input = "hello world!";
            string input2 = "sybzoqdxaekulfjd";
            string expected = "qL84V8anpKPDf8nRJMJ+fA==";
            string expected2 = "Rqm8TCkm8iB9uKj0oti+Uj7Y1IHPpVD9jZdummMgUgE=";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                expected = "xszTZjF/6XCxnIpfJqv7lQ==";
            }

            //// ACT
            string output = AesEncrypt.Encrypt(input);
            string output2 = AesEncrypt.Encrypt(input2);

            //// ASSERT
            Assert.AreEqual(expected, output);
            Assert.AreEqual(expected2, output2);
        }
    }
}
