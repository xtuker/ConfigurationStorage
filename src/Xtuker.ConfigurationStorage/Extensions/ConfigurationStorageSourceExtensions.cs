namespace Xtuker.ConfigurationStorage.Extensions;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xtuker.ConfigurationStorage.Crypto;

/// <summary>
/// Методы расширения <see cref="ConfigurationStorageSource"/>
/// </summary>
public static class ConfigurationStorageSourceExtensions
{
    /// <summary>
    /// Использовать сервис отслеживания изменений конфигурации
    /// </summary>
    public static T UseChangeNotifier<T>(this T source, IConfigurationStorageChangeNotifier storageChangeNotifier, int reloadInterval = 300)
        where T : ConfigurationStorageSource
    {
        source.ChangeNotifier = storageChangeNotifier;
        source.ReloadInterval = reloadInterval;

        return source;
    }

    /// <summary>
    /// Использовать хранилище данных
    /// </summary>
    /// <param name="source">Источник конфигурации</param>
    /// <param name="storage">Криптографический провайдер</param>
    public static T UseStorage<T>(this T source, IConfigurationStorage storage)
        where T : ConfigurationStorageSource
    {
        source.Storage = storage;

        return source;
    }

    /// <summary>
    /// Использовать логгер
    /// </summary>
    /// <param name="source">Источник конфигурации</param>
    /// <param name="logger">Сервис логирования</param>
    public static T UseLogger<T>(this T source, ILogger logger)
        where T : ConfigurationStorageSource
    {
        source.Logger = logger;

        return source;
    }

    /// <summary>
    /// Использовать фабрику логгера
    /// </summary>
    /// <param name="source">Источник конфигурации</param>
    /// <param name="loggerFactory">Фабрика создания сервиса логирования</param>
    public static T UseLoggerFactory<T>(this T source, ILoggerFactory loggerFactory)
        where T : ConfigurationStorageSource
    {
        source.Logger = loggerFactory.CreateLogger<ConfigurationStorageProvider>();

        return source;
    }

    /// <summary>
    /// Использовать автоматическое обновление данных по таймауту
    /// </summary>
    /// <param name="source">Источник конфигурации</param>
    /// <param name="delaySeconds">Задержка перед обновлением данных</param>
    public static T ReloadOnExpiry<T>(this T source, int delaySeconds = 60)
        where T : ConfigurationStorageSource
    {
        source.UseChangeNotifier(new ConfigurationStorageChangeNotifier(TimeSpan.FromSeconds(delaySeconds)));

        return source;
    }

    /// <summary>
    /// Использовать криптографический провайдер
    /// </summary>
    /// <param name="source">Источник конфигурации</param>
    /// <param name="cryptoTransformer">Криптографический провайдер</param>
    public static T UseCryptoTransformer<T>(this T source, IConfigurationCryptoTransformer cryptoTransformer)
        where T : ConfigurationStorageSource
    {
        source.CryptoTransformer = cryptoTransformer;

        return source;
    }

    /// <summary>
    /// Использовать <see cref="AesConfigurationCryptoTransformer"/>
    /// </summary>
    /// <param name="source">Источник конфигурации</param>
    /// <param name="key">ключ шифрования</param>
    public static T UseAesCryptoTransformer<T>(this T source, ReadOnlySpan<byte> key)
        where T : ConfigurationStorageSource
    {
        return source.UseCryptoTransformer(new AesConfigurationCryptoTransformer(key));
    }

    /// <summary>
    /// Использовать <see cref="AesConfigurationCryptoTransformer"/>
    /// <para>
    /// Key: <see cref="ConfigurationStorageSource.CryptoTransformerKeyPath"/>
    /// </para>
    /// </summary>
    /// <param name="source">Источник конфигурации</param>
    /// <param name="configurationSection">Секция конфигурации провайдера</param>
    public static T UseAesCryptoTransformer<T>(this T source, IConfiguration configurationSection)
        where T : ConfigurationStorageSource
    {
        var keyStr = configurationSection[ConfigurationStorageSource.CryptoTransformerKeyPath] ??
            throw new ArgumentNullException(ConfigurationStorageSource.CryptoTransformerKeyPath, "Не найден ключ шифрования");
        var key = Convert.FromBase64String(keyStr);

        return source.UseAesCryptoTransformer(key);
    }
}