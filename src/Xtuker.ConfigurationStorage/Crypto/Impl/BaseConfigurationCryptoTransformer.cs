namespace Xtuker.ConfigurationStorage.Crypto;

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
        where T : IConfigurationData
    {
        if (configurationData is {Encrypted: true, Value: not null})
        {
            using var alg = CreateSymmetricAlgorithm();
            var value = configurationData.Value;
            configurationData.Value = EncryptInternal(alg, value, silent);
        }

        return configurationData;
    }

    /// <inheritdoc />
    public T? Decrypt<T>(T? configurationData, bool silent = true)
        where T : IConfigurationData
    {
        if (configurationData is {Encrypted: true, Value: not null})
        {
            using var alg = CreateSymmetricAlgorithm();
            var value = configurationData.Value;
            configurationData.Value = DecryptInternal(alg, value, silent);
        }

        return configurationData;
    }

    /// <inheritdoc />
    public IEnumerable<T> Encrypt<T>(IEnumerable<T> configurationDatas, bool silent = true)
        where T : class, IConfigurationData
    {
        using var alg = CreateSymmetricAlgorithm();
        foreach (var configurationData in configurationDatas)
        {
            if (configurationData is {Encrypted: true, Value: not null})
            {
                var value = configurationData.Value;
                configurationData.Value = EncryptInternal(alg, value, silent);
            }

            yield return configurationData;
        }
    }

    /// <inheritdoc />
    public IEnumerable<T> Decrypt<T>(IEnumerable<T> configurationDatas, bool silent = true)
        where T : class, IConfigurationData
    {
        using var alg = CreateSymmetricAlgorithm();
        foreach (var configurationData in configurationDatas)
        {
            if (configurationData is {Encrypted: true, Value: not null})
            {
                var value = configurationData.Value;
                configurationData.Value = DecryptInternal(alg, value, silent);
            }

            yield return configurationData;
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
            GenerateIV(alg);
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
            var payload = JsonSerializer.Deserialize<EncryptedPayload>(cipherText);

            using var decryptor = alg.CreateDecryptor(alg.Key, payload!.Salt);
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

    /// <summary>
    /// Сгенерировать IV
    /// </summary>
    /// <remarks>По умолчанию: <see cref="SymmetricAlgorithm.GenerateIV"/> </remarks>
    // ReSharper disable once InconsistentNaming
    protected virtual void GenerateIV(SymmetricAlgorithm alg)
    {
        alg.GenerateIV();
    }

    private record struct EncryptedPayload(byte[] Data, byte[] Salt);
}