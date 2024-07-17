namespace Xtuker.ConfigurationStorage.Crypto;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;

/// <summary>
/// <see cref="IConfigurationCryptoTransformer"/> with AES algorithm
/// </summary>
internal sealed class AesConfigurationCryptoTransformer : BaseConfigurationCryptoTransformer
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private readonly byte[] _key;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="key">secret key</param>
    public AesConfigurationCryptoTransformer(ReadOnlySpan<byte> key)
    {
        if (key.Length == 32)
        {
            _key = key.ToArray();
        }
        else
        {
            throw new ArgumentException("Incorrect key length");
        }
    }

    /// <inheritdoc />
    protected override SymmetricAlgorithm CreateSymmetricAlgorithm()
    {
        var aes = Aes.Create();
        aes.Key = _key;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        return aes;
    }
}