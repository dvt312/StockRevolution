// Copyright(c) Daniel Veintimilla 2016.

#region usings

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace CSharpAssignment.Util
{
    /// <summary>Encrypts and Decrypts a string</summary>
    [ExcludeFromCodeCoverage]
    public class CryptoUtil
    {
        private static CryptoUtil _instance = null;

        protected CryptoUtil() { }

        public static CryptoUtil Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CryptoUtil();
                return _instance;
            }
        }

        /// <summary>
        /// Sets the instance. Just used in tests.
        /// </summary>
        /// <param name="instance">The instance.</param>
        protected static void setTestingInstance(CryptoUtil instance)
        {
            _instance = instance;
        }

        /// <summary>Encrypts a string using AES algorithm and a secret key.</summary>
        /// <param name="textToEncrypt">The string to encrypt</param>
        /// <returns>The encrypted key.</returns>
        public virtual string Encrypt(string textToEncrypt)
        {
            const string key = "CR1PT0S70CKK3Y";
            var bytes = Encoding.Unicode.GetBytes(textToEncrypt);
            using (var aesEncryptor = Aes.Create())
            {
                var rfc =
                    new Rfc2898DeriveBytes(
                        key,
                        new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                if (aesEncryptor == null) return textToEncrypt;
                aesEncryptor.Key = rfc.GetBytes(32);
                aesEncryptor.IV = rfc.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aesEncryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                        cs.Close();
                    }
                    textToEncrypt = Convert.ToBase64String(ms.ToArray());
                }
            }
            return textToEncrypt;
        }

        /// <summary>Decrypts a string using AES algorithm and a secret key.</summary>
        /// <param name="textToDecrypt">The string to decrypt</param>
        /// <returns>The decrypted key.</returns>
        public virtual string Decrypt(string textToDecrypt)
        {
            const string key = "CR1PT0S70CKK3Y";
            var bytes = Convert.FromBase64String(textToDecrypt);
            using (var aesEncryptor = Aes.Create())
            {
                var rfc =
                    new Rfc2898DeriveBytes(
                        key,
                        new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                if (aesEncryptor == null) return textToDecrypt;
                aesEncryptor.Key = rfc.GetBytes(32);
                aesEncryptor.IV = rfc.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aesEncryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytes, 0, bytes.Length);
                        cs.Close();
                    }
                    textToDecrypt = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return textToDecrypt;
        }
    }
}