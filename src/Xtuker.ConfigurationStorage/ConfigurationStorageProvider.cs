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
/// Configuration provider with loading configs from database
/// </summary>
internal sealed class ConfigurationStorageProvider : ConfigurationProvider, IDisposable
{
    private readonly object _locker = new();
    private static readonly ReadOnlyDictionary<string, string?> EmptyData = new (new Dictionary<string, string?>());

    private readonly IConfigurationStorage _storage;
    private readonly ILogger _logger;
    private readonly IDisposable? _changeTokenRegistration;

    internal IConfigurationCryptoTransformer? CryptoTransformer { get; }

    internal IConfigurationStorageChangeNotificationService ChangeNotificationService { get; }

    /// <inheritdoc />
    public ConfigurationStorageProvider(ConfigurationStorageSource storageSource)
    {
        _storage = storageSource.Storage ?? throw new ArgumentException(nameof(ConfigurationStorageSource.Storage));
        _logger = storageSource.Logger;

        CryptoTransformer = storageSource.CryptoTransformer;
        ChangeNotificationService = storageSource.ChangeNotifier ?? new ConfigurationStorageChangeNotificationService();

        _changeTokenRegistration = ChangeToken.OnChange(() => ChangeNotificationService.CreateChangeToken(), Load);
    }

    public void Reload()
    {
        ChangeNotificationService.NotifyChange();
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
                var data = CryptoTransformer == null ? _storage.GetData() : CryptoTransformer.Decrypt(_storage.GetData());
                Data = data.Where(x => !string.IsNullOrEmpty(x?.Key))
                    .ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);

                OnReload();
            }
        }
        catch(Exception e)
        {
            Data = EmptyData;
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
        ChangeNotificationService.Dispose();
    }
}