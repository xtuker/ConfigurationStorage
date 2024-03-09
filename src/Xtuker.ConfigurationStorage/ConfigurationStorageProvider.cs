namespace Xtuker.ConfigurationStorage
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Поставщик конфигурации из базы данных
    /// </summary>
    internal sealed class ConfigurationStorageProvider : ConfigurationProvider, IDisposable
    {
        private readonly object _locker = new();
        
        private readonly IDisposable? _changeTokenRegistration;
        private readonly IConfigurationStorageChangeNotifier? _changeNotifier;
        private readonly ILogger? _logger;
        
        /// <summary>
        /// Хранилище конфигурации
        /// </summary>
        public IConfigurationStorage Storage { get; }

        private readonly IDictionary<string, string?> _emptyData = new ReadOnlyDictionary<string, string?>(new Dictionary<string, string?>());

        /// <inheritdoc />
        public ConfigurationStorageProvider(ConfigurationStorageSource storageSource)
        {
            Storage = storageSource.Storage ?? throw new ArgumentException(nameof(ConfigurationStorageSource.Storage));
            _logger = storageSource.Logger;

            if (storageSource.ChangeNotifier != null)
            {
                _changeNotifier = new ConfigurationStorageChangeNotifier(TimeSpan.FromSeconds(storageSource.ReloadInterval));
                _changeTokenRegistration = ChangeToken.OnChange(() => _changeNotifier.CreateChangeToken(), Load);
            }
        }

        public void Reload()
        {
            _changeNotifier?.NotifyChange();
        }

        /// <inheritdoc />
        public override void Set(string key, string? value)
        {
            // NOT SUPPORTED SAVE FROM CONFIGURATION PROVIDER
        }

        /// <inheritdoc />
        public override void Load()
        {
            _logger?.LogInformation("Configuration loading...");
            try
            {
                if (Monitor.TryEnter(_locker))
                {
                    var data = Storage.GetData();
                    Data = data.Where(x => !string.IsNullOrEmpty(x?.Key))
                        .ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);

                    OnReload();
                }
            }
            catch(Exception e)
            {
                Data = _emptyData;
                _logger?.LogError(e, "Load configuration error");
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
            _changeNotifier?.Dispose();
        }
    }
}