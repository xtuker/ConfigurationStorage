namespace Xtuker.ConfigurationStorage.Crypto
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Security.Cryptography;

    /// <summary>
    /// Провайдер шифрования использующий алгоритм AES
    /// </summary>
    internal sealed class AesConfigurationCryptoTransformer : BaseConfigurationCryptoTransformer
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly byte[] _key;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="key">Ключ шифрования</param>
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
}