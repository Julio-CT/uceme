namespace Uceme.Foundation.Tests.Tools
{
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
            string expected = "qL84V8anpKPDf8nRJMJ+fA==";

            //// ACT
            string output = AesEncrypt.Encrypt(input);

            //// ASSERT
            Assert.AreEqual(expected, output);
        }
    }
}
