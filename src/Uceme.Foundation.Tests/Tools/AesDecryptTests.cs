namespace Uceme.Foundation.Tests.Tools
{
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uceme.Foundation.Tools;

    [TestClass]
    public class AesDecryptTests
    {
        [TestMethod]
        [TestCategory("MockTests")]
        [Ignore("ski pre-season")]
        public async Task EncryptShouldDecryptWithoutIssuesAsync()
        {
            //// ARRRANGE
            string input = "hello world!";
            string expected = "qL84V8anpKPDf8nRJMJ+fA==";
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                expected = "qL84V8anpKPDf8nRJMJ+fA==";
            }

            //// ACT
            string output = await AesDecrypt.DecryptAsync(expected).ConfigureAwait(false);

            //// ASSERT
            Assert.AreEqual(input, output);
        }
    }
}
