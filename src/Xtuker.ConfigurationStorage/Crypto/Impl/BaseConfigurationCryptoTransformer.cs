namespace Xtuker.ConfigurationStorage.Crypto
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text.Json;
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
        public T? Encrypt<T>(T? configurationData, bool silent = true)
            where T : class, IConfigurationData
        {
            if (configurationData is {Encrypted: true, Value: not null})
            {
                using var alg = CreateSymmetricAlgorithm();
                configurationData.Value = EncryptInternal(alg, configurationData.Value, silent);
            }

            return configurationData;
        }

        /// <inheritdoc />
        public T? Decrypt<T>(T? sensitiveSetting, bool silent = true)
            where T : class, IConfigurationData
        {
            if (sensitiveSetting is {Encrypted: true, Value: not null})
            {
                using var alg = CreateSymmetricAlgorithm();
                sensitiveSetting.Value = DecryptInternal(alg, sensitiveSetting.Value, silent);
            }

            return sensitiveSetting;
        }

        /// <inheritdoc />
        public IEnumerable<T> Encrypt<T>(IEnumerable<T> sensitiveSettings, bool silent = true)
            where T : class, IConfigurationData
        {
            using var alg = CreateSymmetricAlgorithm();
            foreach (var setting in sensitiveSettings)
            {
                if (setting is {Encrypted: true, Value: not null})
                {
                    setting.Value = EncryptInternal(alg, setting.Value, silent);
                }

                yield return setting;
            }
        }

        /// <inheritdoc />
        public IEnumerable<T> Decrypt<T>(IEnumerable<T> sensitiveSettings, bool silent = true)
            where T : class, IConfigurationData
        {
            using var alg = CreateSymmetricAlgorithm();
            foreach (var setting in sensitiveSettings)
            {
                if (setting is {Encrypted: true, Value: not null})
                {
                    setting.Value = DecryptInternal(alg, setting.Value, silent);
                }

                yield return setting;
            }
        }

        /// <summary>
        /// Зашифровать строку
        /// </summary>
        [return:NotNullIfNotNull(nameof(plainText))]
        protected virtual string? EncryptInternal(SymmetricAlgorithm alg, string? plainText, bool silent)
        {
            if (plainText == null)
            {
                return null;
            }

            try
            {
                alg.GenerateIV();
                using var encryptor = alg.CreateEncryptor();
                using var ms = new MemoryStream();
                using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                return JsonSerializer.Serialize(new EncryptedPayload(ms.ToArray(), alg.IV));
            }
            catch (Exception e)
            {
                Logger?.LogWarning(e, "Encrypt config failed");
                if (!silent)
                {
                    throw;
                }
                return null!;
            }
        }

        /// <summary>
        /// Расшифровать строку
        /// </summary>
        [return:NotNullIfNotNull(nameof(cipherText))]
        protected virtual string? DecryptInternal(SymmetricAlgorithm alg, string? cipherText, bool silent)
        {
            if (cipherText == null)
            {
                return null;
            }
            
            try
            {
                var payload = JsonSerializer.Deserialize<EncryptedPayload>(cipherText)!;

                using var decryptor = alg.CreateDecryptor(alg.Key, payload.Salt);
                using var ms = new MemoryStream(payload.Data);
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);
                
                return sr.ReadToEnd();
            }
            catch (Exception e)
            {
                Logger?.LogWarning(e, "Decrypt config failed");
                if (!silent)
                {
                    throw;
                }
                return null!;
            }
        }

        private class EncryptedPayload
        {
            public EncryptedPayload(byte[] data, byte[] salt)
            {
                Data = data;
                Salt = salt;
            }

            public byte[] Data { get; }
            public byte[] Salt { get; }
        }
    }
}