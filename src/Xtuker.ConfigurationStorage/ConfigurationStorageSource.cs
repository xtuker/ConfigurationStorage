namespace Xtuker.ConfigurationStorage
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Xtuker.ConfigurationStorage.Crypto;

    /// <inheritdoc />
    public class ConfigurationStorageSource : IConfigurationSource
    {
        /// <summary>
        /// Ключ конфигурации для ключа шифрования
        /// </summary>
        public const string CryptoTransformerKeyPath = "ConfigurationStorage:CryptoTransformer:Key";

        /// <summary>
        /// Источник данных
        /// </summary>
        public IConfigurationStorage Storage { get; internal set; } = null!;

        /// <summary>
        /// Криптографический провайдер
        /// </summary>
        public IConfigurationCryptoTransformer? CryptoTransformer { get; internal set; }

        /// <summary>
        /// Сервис логирования
        /// </summary>
        public ILogger? Logger { get; internal set; }

        /// <summary>
        /// Сервис отслеживания изменений конфигурации
        /// </summary>
        public IConfigurationStorageChangeNotifier? ChangeNotifier { get; internal set; }

        /// <summary>
        /// Интервал перезагрузки конфигурации
        /// </summary>
        public int ReloadInterval { get; internal set; }

        /// <inheritdoc />
        public virtual IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConfigurationStorageProvider(this);
        }
    }
}