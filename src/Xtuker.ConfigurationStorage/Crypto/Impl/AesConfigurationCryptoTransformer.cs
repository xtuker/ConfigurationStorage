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
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        private readonly byte[]? _iv;
        
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
            else if (key.Length == 48)
            {
                _key = key.Slice(0, 32).ToArray();
                _iv = key.Slice(32).ToArray();
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
            if (_iv != null)
            {
                aes.IV = _iv;
            }

            return aes;
        }
    }
}