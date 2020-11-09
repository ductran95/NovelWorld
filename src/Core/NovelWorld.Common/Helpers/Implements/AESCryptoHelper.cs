using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NovelWorld.Common.Helpers.Abstractions;

namespace NovelWorld.Common.Helpers.Implements
{
    public class AESCryptoHelper: ICryptoHelper
    {
        private const string SecretKey = "bchBChbBJGDbcjg^&^*15456";
        private static readonly byte[] SecretKeyBytes = Encoding.UTF8.GetBytes(SecretKey);
        
        public string Encrypt(string plainText)
        {
            using var aesAlg = Aes.Create();
            // ReSharper disable once PossibleNullReferenceException
            using var encryptor = aesAlg.CreateEncryptor(SecretKeyBytes, aesAlg.IV);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            var iv = aesAlg.IV;

            var decryptedContent = msEncrypt.ToArray();

            var result = new byte[iv.Length + decryptedContent.Length];

            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

            return Convert.ToBase64String(result);
        }

        public string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);

            using var aesAlg = Aes.Create();
            // ReSharper disable once PossibleNullReferenceException
            using var decryptor = aesAlg.CreateDecryptor(SecretKeyBytes, iv);
            using var msDecrypt = new MemoryStream(cipher);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            var result = srDecrypt.ReadToEnd();

            return result;
        }
    }
}