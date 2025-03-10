using RapidPay.Application.Common;
using RapidPay.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPay.Test
{
    [TestFixture]
    public class EncryptionServiceTests
    {
        private IEncryptionService _encryptionService;
        private const string TestKey = "TESTSecretKeyForAES256Encryption";

        [SetUp]
        public void Setup()
        {
            _encryptionService = new EncryptionService(TestKey);
        }

        [Test]
        public void EncryptAndDecrypt_ReturnsOriginalValue()
        {
            var plainText = "123456789012345";
            var encrypted = _encryptionService.Encrypt(plainText);
            var decrypted = _encryptionService.Decrypt(encrypted);
            Assert.That(decrypted, Is.EqualTo(plainText));
        }

    }
}
