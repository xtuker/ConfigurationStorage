namespace Xtuker.ConfigurationStorage;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xtuker.ConfigurationStorage.Crypto;

/// <inheritdoc />
public class ConfigurationStorageSource : IConfigurationSource
{
    /// <summary>
    /// .ctor
    /// </summary>
    public ConfigurationStorageSource(IConfigurationStorage storage)
    {
        Storage = storage;
    }

    /// <summary>
    /// Secret key for <see cref="IConfigurationCryptoTransformer"/>
    /// </summary>
    public const string CryptoTransformerKeyPath = "ConfigurationStorage:CryptoTransformer:Key";

    /// <inheritdoc cref="IConfigurationStorage"/>
    public IConfigurationStorage Storage { get; }

    /// <inheritdoc cref="IConfigurationCryptoTransformer"/>
    public IConfigurationCryptoTransformer? CryptoTransformer { get; internal set; }

    /// <summary>
    /// Logger instance
    /// </summary>
    public ILogger Logger { get; internal set; } = NullLogger.Instance;

    /// <inheritdoc cref="IConfigurationStorageChangeNotificationService"/>
    public IConfigurationStorageChangeNotificationService? ChangeNotifier { get; internal set; }

    /// <inheritdoc />
    public virtual IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        if (CryptoTransformer is BaseConfigurationCryptoTransformer baseConfigurationCryptoTransformer)
        {
            baseConfigurationCryptoTransformer.Logger = Logger;
        }

        return new ConfigurationStorageProvider(this);
    }
}