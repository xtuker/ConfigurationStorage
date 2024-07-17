namespace Xtuker.ConfigurationStorage;

using System;
using System.Threading;
using Microsoft.Extensions.Primitives;

/// <summary>
/// Сервис обновления конфигурации по таймеру
/// </summary>
internal sealed class ConfigurationStorageChangeNotifier : IConfigurationStorageChangeNotifier
{
    private readonly Func<CancellationTokenSource> _factory;
    private volatile CancellationTokenSource? _cancellationTokenSource;

    /// <summary>
    /// .ctor
    /// </summary>
    public ConfigurationStorageChangeNotifier()
    {
        _factory = () => new CancellationTokenSource();
    }

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="refreshInterval">Интервал изменений</param>
    public ConfigurationStorageChangeNotifier(TimeSpan refreshInterval)
    {
        _factory = () => new CancellationTokenSource(refreshInterval);
    }

    public void NotifyChange()
    {
        _cancellationTokenSource?.Cancel();
    }

    public IChangeToken CreateChangeToken()
    {
        var previousToken = Interlocked.Exchange(ref _cancellationTokenSource, _factory());
        previousToken?.Dispose();
        return new CancellationChangeToken(_cancellationTokenSource!.Token);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _cancellationTokenSource?.Dispose();
    }
}