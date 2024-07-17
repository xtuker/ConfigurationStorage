namespace Xtuker.ConfigurationStorage.Extensions;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xtuker.ConfigurationStorage.Crypto;

/// <summary>
/// Extensions for <see cref="ConfigurationStorageSource"/>
/// </summary>
public static class ConfigurationStorageSourceExtensions
{
    /// <summary>
    /// Use change notification service
    /// </summary>
    /// <seealso cref="IConfigurationStorageChangeNotificationService"/>
    public static T UseChangeNotificationService<T>(this T source, IConfigurationStorageChangeNotificationService storageChangeNotificationService)
        where T : ConfigurationStorageSource
    {
        source.ChangeNotifier = storageChangeNotificationService;

        return source;
    }

    /// <summary>
    /// Use logger
    /// </summary>
    public static T UseLogger<T>(this T source, ILogger logger)
        where T : ConfigurationStorageSource
    {
        source.Logger = logger;

        return source;
    }

    /// <summary>
    /// Use logger factory
    /// </summary>
    public static T UseLoggerFactory<T>(this T source, ILoggerFactory loggerFactory)
        where T : ConfigurationStorageSource
    {
        source.Logger = loggerFactory.CreateLogger<ConfigurationStorageProvider>();

        return source;
    }

    /// <summary>
    /// Reload configuration on timeout
    /// </summary>
    /// <remarks>Default: 60 seconds</remarks>
    /// <param name="source">configuration source</param>
    /// <param name="delaySeconds">delay in seconds</param>
    public static T ReloadOnExpiry<T>(this T source, int delaySeconds = 60)
        where T : ConfigurationStorageSource
    {
        source.UseChangeNotificationService(new ConfigurationStorageChangeNotificationService(TimeSpan.FromSeconds(delaySeconds)));

        return source;
    }

    /// <summary>
    /// Use custom configuration crypto transformer
    /// </summary>
    /// <seealso cref="IConfigurationCryptoTransformer"/>
    public static T UseCryptoTransformer<T>(this T source, IConfigurationCryptoTransformer cryptoTransformer)
        where T : ConfigurationStorageSource
    {
        source.CryptoTransformer = cryptoTransformer;

        return source;
    }

    /// <summary>
    /// Use AES configuration crypto transformer
    /// </summary>
    /// <param name="source">configuration source</param>
    /// <param name="key">secret key</param>
    /// <seealso cref="IConfigurationCryptoTransformer"/>
    public static T UseAesCryptoTransformer<T>(this T source, ReadOnlySpan<byte> key)
        where T : ConfigurationStorageSource
    {
        return source.UseCryptoTransformer(new AesConfigurationCryptoTransformer(key));
    }

    /// <summary>
    /// Use AES configuration crypto transformer
    /// <para>
    /// Using secret key from configuration <see cref="ConfigurationStorageSource.CryptoTransformerKeyPath"/>
    /// </para>
    /// </summary>
    /// <param name="source">configuration source</param>
    /// <param name="configurationSection">root configuration section with secret key</param>
    /// <seealso cref="IConfigurationCryptoTransformer"/>
    /// <exception cref="ArgumentNullException">secret key does not set</exception>
    public static T UseAesCryptoTransformer<T>(this T source, IConfiguration configurationSection)
        where T : ConfigurationStorageSource
    {
        var keyStr = configurationSection[ConfigurationStorageSource.CryptoTransformerKeyPath] ??
            throw new ArgumentNullException(ConfigurationStorageSource.CryptoTransformerKeyPath, "secret key does not set");
        var key = Convert.FromBase64String(keyStr);

        return source.UseAesCryptoTransformer(key);
    }
}