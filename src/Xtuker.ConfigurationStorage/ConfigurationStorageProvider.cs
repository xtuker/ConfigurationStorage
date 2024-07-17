namespace Xtuker.ConfigurationStorage;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Xtuker.ConfigurationStorage.Crypto;

/// <summary>
/// Поставщик конфигурации из базы данных
/// </summary>
internal sealed class ConfigurationStorageProvider : ConfigurationProvider, IDisposable
{
    private readonly object _locker = new();
    private const int DefaultInterval = 300;

    private readonly IDisposable? _changeTokenRegistration;
    private readonly ILogger _logger;

    /// <summary>
    /// Хранилище конфигурации
    /// </summary>
    internal IConfigurationStorage Storage { get; }

    /// <summary>
    /// Провайдер криптографических преобразований
    /// </summary>
    internal IConfigurationCryptoTransformer? CryptoTransformer { get; }

    /// <summary>
    /// Сервис отслеживания изменений конфигурации
    /// </summary>
    internal IConfigurationStorageChangeNotifier ChangeNotifier { get; }

    private readonly IDictionary<string, string?> _emptyData = new ReadOnlyDictionary<string, string?>(new Dictionary<string, string?>());

    /// <inheritdoc />
    public ConfigurationStorageProvider(ConfigurationStorageSource storageSource)
    {
        Storage = storageSource.Storage ?? throw new ArgumentException(nameof(ConfigurationStorageSource.Storage));
        CryptoTransformer = storageSource.CryptoTransformer;
        _logger = storageSource.Logger;

        ChangeNotifier = storageSource.ChangeNotifier ?? new ConfigurationStorageChangeNotifier();
        _changeTokenRegistration = ChangeToken.OnChange(() => ChangeNotifier.CreateChangeToken(), Load);
    }

    public void Reload()
    {
        ChangeNotifier.NotifyChange();
    }

    /// <inheritdoc />
    public override void Set(string key, string? value)
    {
        // NOT SUPPORTED SAVE FROM CONFIGURATION PROVIDER
    }

    /// <inheritdoc />
    public override void Load()
    {
        _logger.LogDebug("Configuration loading...");
        try
        {
            if (Monitor.TryEnter(_locker))
            {
                var data = CryptoTransformer == null ? Storage.GetData() : CryptoTransformer.Decrypt(Storage.GetData());
                Data = data.Where(x => !string.IsNullOrEmpty(x?.Key))
                    .ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);

                OnReload();
            }
        }
        catch(Exception e)
        {
            Data = _emptyData;
            _logger.LogError(e, "Load configuration error");
        }
        finally
        {
            if (Monitor.IsEntered(_locker))
            {
                Monitor.Exit(_locker);
            }
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _changeTokenRegistration?.Dispose();
        ChangeNotifier?.Dispose();
    }
}