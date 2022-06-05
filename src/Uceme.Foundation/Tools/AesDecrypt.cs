namespace Uceme.Foundation.Tools
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public class AesDecrypt
    {
        public static async Task<string> DecryptAsync(string encryptedString)
        {
            string decryptedMessage = string.Empty;
            try
            {
                byte[] byteArray = Convert.FromBase64String(encryptedString);
                MemoryStream stream = new MemoryStream(byteArray);

                using (Aes aes = Aes.Create())
                {
                    byte[] key =
                    {
                        0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
                        0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16,
                    };
                    aes.Key = key;

                    byte[] iv =
                    {
                        0x21, 0x22, 0x23, 024, 0x25, 0x26, 0x27, 0x28,
                        0x29, 0x20, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16,
                    };

                    using (CryptoStream cryptoStream = new(
                        stream,
                        aes.CreateDecryptor(key, iv),
                        CryptoStreamMode.Read))
                    {
                        using (StreamReader decryptReader = new(cryptoStream))
                        {
                            decryptedMessage = (await decryptReader.ReadToEndAsync().ConfigureAwait(false)).Replace("\n", string.Empty, StringComparison.InvariantCultureIgnoreCase);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The decryption failed. {ex}");
            }

            return decryptedMessage;
        }
    }
}
