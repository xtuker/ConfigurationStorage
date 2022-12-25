namespace Xtuker.ConfigurationStorage.Crypto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Security.Cryptography;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Провайдер шифрования
    /// </summary>
    public abstract class BaseConfigurationCryptoTransformer : IConfigurationCryptoTransformer
    {
        /// <summary>
        /// Фабричный метод создания симметричного алгоритма шифрования
        /// </summary>
        protected abstract SymmetricAlgorithm CreateSymmetricAlgorithm();
        
        /// <summary>
        /// Логгер
        /// </summary>
        protected ILogger? Logger { get; set; }

        /// <inheritdoc />
        public T? TryEncrypt<T>(T? configurationData)
            where T : class, IConfigurationData
        {
            if (configurationData is {Encrypted: true, Value: {}})
            {
                using var alg = CreateSymmetricAlgorithm();
                using var encryptor = alg.CreateEncryptor();
                
                configurationData.Value = EncryptInternal(encryptor, configurationData.Value);
            }

            return configurationData;
        }

        /// <inheritdoc />
        public T? TryDecrypt<T>(T? sensitiveSetting)
            where T : class, IConfigurationData
        {
            if (sensitiveSetting is {Encrypted: true, Value: {}})
            {
                using var alg = CreateSymmetricAlgorithm();
                using var decryptor = alg.CreateDecryptor();

                sensitiveSetting.Value = DecryptInternal(decryptor, sensitiveSetting.Value);
            }

            return sensitiveSetting;
        }

        /// <inheritdoc />
        public IEnumerable<T> Encrypt<T>(IEnumerable<T> sensitiveSettings)
            where T : class, IConfigurationData
        {
            using var alg = CreateSymmetricAlgorithm();
            using var encryptor = alg.CreateEncryptor();

            foreach (var setting in sensitiveSettings)
            {
                if (setting is {Encrypted: true, Value: {}})
                {
                    setting.Value = EncryptInternal(encryptor, setting.Value);
                }

                yield return setting;
            }
        }

        /// <inheritdoc />
        public IEnumerable<T> Decrypt<T>(IEnumerable<T> sensitiveSettings)
            where T : class, IConfigurationData
        {
            using var alg = CreateSymmetricAlgorithm();
            using var decryptor = alg.CreateDecryptor();

            foreach (var setting in sensitiveSettings)
            {
                if (setting is {Encrypted: true, Value: {}})
                {
                    setting.Value = DecryptInternal(decryptor, setting.Value);
                }

                yield return setting;
            }
        }

        /// <summary>
        /// Зашифровать строку
        /// </summary>
        [return:NotNullIfNotNull(nameof(plainText))]
        protected virtual string? EncryptInternal(ICryptoTransform encryptor, string? plainText)
        {
            if (plainText == null)
            {
                return null;
            }

            try
            {
                using var ms = new MemoryStream();
                using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                Logger?.LogWarning(e, "Encrypt config failed");
                return null!;
            }
        }

        /// <summary>
        /// Расшифровать строку
        /// </summary>
        [return:NotNullIfNotNull(nameof(cipherText))]
        protected virtual string? DecryptInternal(ICryptoTransform decryptor, string? cipherText)
        {
            if (cipherText == null)
            {
                return null;
            }
            
            try
            {
                using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);
                
                return sr.ReadToEnd();
            }
            catch (Exception e)
            {
                Logger?.LogWarning(e, "Decrypt config failed");
                return null!;
            }
        }
    }
}