namespace Uceme.Foundation.Tools
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public class AesEncrypt
    {
        public static string Encrypt(string textToEncrypt)
        {
            string result = string.Empty;
            try
            {
                using (MemoryStream memoryStream = new())
                {
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
                            memoryStream,
                            aes.CreateEncryptor(aes.Key, iv),
                            CryptoStreamMode.Write))
                        {
                            using (StreamWriter encryptWriter = new(cryptoStream))
                            {
                                encryptWriter.WriteLine(textToEncrypt);
                            }

                            var encrypted = memoryStream.ToArray();
                            result = Convert.ToBase64String(encrypted);
                        }
                    }
                }

                Console.WriteLine("The file was encrypted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The encryption failed. {ex}");
            }

            return result;
        }
    }
}
