using RapidPay.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Application.Utils
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _fixedIv;

        public EncryptionService(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            _key = Encoding.UTF8.GetBytes(key);
            _fixedIv = new byte[16];
        }

        public string Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _fixedIv; // Fixed IV for deterministic encryption.
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(input);
            }
            var encryptedBytes = ms.ToArray();
            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var fullCipher = Convert.FromBase64String(input);
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _fixedIv;
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(fullCipher);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
