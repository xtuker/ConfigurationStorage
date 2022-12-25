namespace Xtuker.ConfigurationStorage
{
    using System;
    using System.Threading;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Сервис обновления конфигурации по таймеру
    /// </summary>
    internal sealed class ConfigurationStorageChangeNotifier : IConfigurationStorageChangeNotifier
    {
        private readonly Timer _timer;
        private volatile CancellationTokenSource? _cancellationTokenSource;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="refreshInterval">Интервал изменений</param>
        public ConfigurationStorageChangeNotifier(TimeSpan refreshInterval)
        {
            _timer = new Timer(Change, null, TimeSpan.Zero, refreshInterval);
        }

        private void Change(object? state)
        {
            _cancellationTokenSource?.Cancel();
        }

        /// <inheritdoc />
        public IChangeToken NotifyChange()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            return new CancellationChangeToken(_cancellationTokenSource.Token);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _timer?.Dispose();
            _cancellationTokenSource?.Dispose();
        }
    }
}