namespace Xtuker.ConfigurationStorage.Crypto
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Провайдер криптографических преобразований для <see cref="IConfigurationData"/>
    /// </summary>
    public interface IConfigurationCryptoTransformer
    {
        /// <summary>
        /// Зашифровать значение, если установлен флаг <see cref="IConfigurationData.Encrypted"/>
        /// </summary>
        /// <param name="configurationData">Настройки</param>
        [return: NotNullIfNotNull(nameof(configurationData))]
        public T? TryEncrypt<T>(T? configurationData)
            where T : class, IConfigurationData;

        /// <summary>
        /// Расшифровать значение, если установлен флаг <see cref="IConfigurationData.Encrypted"/>
        /// </summary>
        /// <param name="configurationData">Настройки</param>
        [return: NotNullIfNotNull(nameof(configurationData))]
        public T? TryDecrypt<T>(T? configurationData)
            where T : class, IConfigurationData;
        
        /// <summary>
        /// Зашифровать значения, если установлен флаг <see cref="IConfigurationData.Encrypted"/>
        /// </summary>
        /// <param name="configurationDatas">Настройки</param>
        public IEnumerable<T> Encrypt<T>(IEnumerable<T> configurationDatas)
            where T : class, IConfigurationData;
        
        /// <summary>
        /// Расшифровать значения, если установлен флаг <see cref="IConfigurationData.Encrypted"/>
        /// </summary>
        /// <param name="configurationDatas">Настройки</param>
        public IEnumerable<T> Decrypt<T>(IEnumerable<T> configurationDatas)
            where T : class, IConfigurationData;
    }
}