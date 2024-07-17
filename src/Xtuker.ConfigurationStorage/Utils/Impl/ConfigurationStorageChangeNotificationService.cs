namespace Xtuker.ConfigurationStorage;

using System;
using System.Threading;
using Microsoft.Extensions.Primitives;

/// <inheritdoc />
internal sealed class ConfigurationStorageChangeNotificationService : IConfigurationStorageChangeNotificationService
{
    private readonly Func<CancellationTokenSource> _factory;
    private volatile CancellationTokenSource? _cancellationTokenSource;

    public ConfigurationStorageChangeNotificationService()
    {
        _factory = () => new CancellationTokenSource();
    }

    public ConfigurationStorageChangeNotificationService(TimeSpan refreshInterval)
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