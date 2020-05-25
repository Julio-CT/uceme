namespace UCEME.Utilidades
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class Encodificacion
    {/// <summary>
     /// Takes a string and generates a hash value of 16 bytes.
     /// </summary>
     /// <param name="str">The string to be hashed</param>
     /// <param name="passwordFormat">Selects the hashing algorithm used. Accepted values are "sha1" and "md5".</param>
     /// <returns>A hex string of the hashed password.</returns>
        public static string EncodeString(string str, string passwordFormat)
        {
            if (str == null)
                return null;

            var ae = new ASCIIEncoding();
            byte[] result;
            switch (passwordFormat)
            {
                case "sha1":
                    SHA1 sha1 = new SHA1CryptoServiceProvider();
                    result = sha1.ComputeHash(ae.GetBytes(str));
                    sha1.Dispose();
                    break;

                case "md5":
                    MD5 md5 = new MD5CryptoServiceProvider();
                    result = md5.ComputeHash(ae.GetBytes(str));
                    md5.Dispose();
                    break;

                default:
                    throw new ArgumentException("Invalid format value. Accepted values are 'sha1' and 'md5'.", nameof(passwordFormat));
            }

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            var sb = new StringBuilder(16);
            foreach (byte t in result)
            {
                _ = sb.Append(t.ToString("x2", CultureInfo.CurrentCulture));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Takes a string and generates a hash value of 16 bytes.  Uses "md5" by default.
        /// </summary>
        /// <param name="str">The string to be hashed</param>
        /// <returns>A hex string of the hashed password.</returns>
        public static string EncodeString(string str)
        {
            return EncodeString(str, "md5");
        }

        /// <summary>
        /// Takes a string and generates a hash value of 16 bytes.
        /// </summary>
        /// <param name="buffer">The string to be hashed</param>
        /// <param name="passwordFormat">Selects the hashing algorithm used. Accepted values are "sha1" and "md5".</param>
        /// <returns>A string of the hashed password.</returns>
        public static string EncodeBinary(byte[] buffer, string passwordFormat)
        {
            if (buffer == null)
                return null;

            byte[] result;
            switch (passwordFormat)
            {
                case "sha1":
                    SHA1 sha1 = new SHA1CryptoServiceProvider();
                    result = sha1.ComputeHash(buffer);
                    sha1.Dispose();
                    break;

                case "md5":
                    MD5 md5 = new MD5CryptoServiceProvider();
                    result = md5.ComputeHash(buffer);
                    md5.Dispose();
                    break;

                default:
                    throw new ArgumentException("Invalid format value. Accepted values are 'sha1' and 'md5'.", nameof(passwordFormat));
            }

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            var sb = new StringBuilder(16);
            foreach (byte t in result)
            {
                _ = sb.Append(t.ToString("x2", CultureInfo.CurrentCulture));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Encodes the buffer using the default cryptographic provider.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static string EncodeBinary(byte[] buffer)
        {
            return EncodeBinary(buffer, "md5");
        }

        /// <summary>
        /// Creates a random alphanumeric password on dimension (length).
        /// </summary>
        /// <param name="length">The number of characters in the password</param>
        /// <returns>The generated password</returns>
        public static string CreateRandomPassword(int length = 8)
        {
            var rnd = new Random(Convert.ToInt32(DateTime.Now.Millisecond));  //Creates the seed from the time
            var password = "";
            while (password.Length < length)
            {
                var newChar = Convert.ToChar((int)((122 - 48 + 1) * rnd.NextDouble() + 48));
                if (newChar >= 'A' & newChar <= 'Z' | newChar >= 'a' & newChar <= 'z' | newChar >= '0' & newChar <= '9')
                    password += newChar;
            }
            return password;
        }

        /// <summary>
        /// Takes a text message and encrypts it using a password as a key.
        /// </summary>
        /// <param name="plainMessage">A text to encrypt.</param>
        /// <param name="password">The password to encrypt the message with.</param>
        /// <returns>Encrypted string.</returns>
        /// <remarks>This method uses TripleDES symmmectric encryption.</remarks>
        public static string EncodeMessageWithPassword(string plainMessage, string password)
        {
            if (plainMessage == null)
                throw new ArgumentNullException(nameof(plainMessage), "The message cannot be null");

            var des = new TripleDESCryptoServiceProvider
            {
                IV = new byte[8]
            };

            //Creates the key based on the password and stores it in a byte array.
            var pdb = new PasswordDeriveBytes(password, Array.Empty<byte>());
            des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);

            var ms = new MemoryStream(plainMessage.Length * 2);
            var encStream = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            var plainBytes = Encoding.UTF8.GetBytes(plainMessage);
            encStream.Write(plainBytes, 0, plainBytes.Length);
            encStream.FlushFinalBlock();
            var encryptedBytes = new byte[ms.Length];
            ms.Position = 0;
            _ = ms.Read(encryptedBytes, 0, (int)ms.Length);
            encStream.Close();

            des.Dispose();
            pdb.Dispose();

            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Takes an encrypted message using TripleDES and a password as a key and converts it to the original text message.
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message to decode.</param>
        /// <param name="password">The password to decode the message.</param>
        /// <returns>The Decrypted message</returns>
        /// <remarks>This method uses TripleDES symmmectric encryption.</remarks>
        public static string DecodeMessageWithPassword(string encryptedMessage, string password)
        {
            if (encryptedMessage == null)
                throw new ArgumentNullException(nameof(encryptedMessage), "The encrypted message cannot be null");

            var des = new TripleDESCryptoServiceProvider
            {
                IV = new byte[8]
            };

            //Creates the key based on the password and stores it in a byte array.
            var pdb = new PasswordDeriveBytes(password, Array.Empty<byte>());
            des.Key = pdb.CryptDeriveKey("RC2", "MD5", 128, new byte[8]);

            //This line protects the + signs that get replaced by spaces when the parameter is not urlencoded when sent.
            encryptedMessage = encryptedMessage.Replace(" ", "+");
            var ms = new MemoryStream(encryptedMessage.Length * 2);
            var decStream = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            des.Dispose();
            pdb.Dispose();

            byte[] plainBytes;
            try
            {
                var encBytes = Convert.FromBase64String(Convert.ToString(encryptedMessage, CultureInfo.InvariantCulture));
                decStream.Write(encBytes, 0, encBytes.Length);
                decStream.FlushFinalBlock();
                plainBytes = new byte[ms.Length];
                ms.Position = 0;
                _ = ms.Read(plainBytes, 0, (int)ms.Length);
                decStream.Close();
            }
            catch (CryptographicException e)
            {
                throw new ApplicationException("Cannot decrypt message.  Possibly, the password is wrong", e);
            }

            return Encoding.UTF8.GetString(plainBytes);
        }

        //método que es llamado con un string como parametro (password) y devuelve un string con el calculo SHA1 del parametro de entrada
        //vamos, que encripta la contraseña con el algoritmo SHA1
        public static string GetSha1(string passw)
        {
            //si no hay password a encriptar (null) se devuelve null (fácil, eh?)
            if (passw == null) return null;

            //si sí que hay password a encriptar:
            var sha1 = SHA1.Create();
            var textOriginal = Encoding.Default.GetBytes(passw);
            var hash = sha1.ComputeHash(textOriginal);
            var cadena = new StringBuilder();
            foreach (var i in hash)
            {
                _ = cadena.AppendFormat(CultureInfo.CurrentCulture, "{0:x2}", i);
            }

            sha1.Dispose();
            return cadena.ToString();
        }
    }
}