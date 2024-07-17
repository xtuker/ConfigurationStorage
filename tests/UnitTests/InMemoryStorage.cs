namespace UnitTests;

using Microsoft.Extensions.Primitives;
using Xtuker.ConfigurationStorage;

internal class InMemoryStorage : IConfigurationStorage
{
    private readonly IDictionary<string, TestConfigurationData> _internalStorage;

    public InMemoryStorage(IEnumerable<TestConfigurationData> data)
    {
        _internalStorage = data.GroupBy(x => x.Key).ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<IConfigurationData> GetData()
    {
        return _internalStorage.Values.Select(x => x with {Value = x.Value}).ToList();
    }

    public virtual void SaveData(TestConfigurationData config)
    {
        _internalStorage[config.Key] = config with {Value = config.Value};
    }

    public string? Get(string key)
    {
        return _internalStorage.TryGetValue(key, out var value) ? value.Value : null;
    }
}

internal class InMemoryStorageWithNotification : InMemoryStorage
{
    public IConfigurationStorageChangeNotificationService ChangeNotificationService { get; }

    public InMemoryStorageWithNotification(IEnumerable<TestConfigurationData> data)
        : base(data)
    {
        ChangeNotificationService = new NotificationService();
    }

    public override void SaveData(TestConfigurationData config)
    {
        base.SaveData(config);

        ChangeNotificationService.NotifyChange();
    }

    private class NotificationService : IConfigurationStorageChangeNotificationService
    {
        private readonly Func<CancellationTokenSource> _factory;
        private volatile CancellationTokenSource? _cancellationTokenSource;

        public NotificationService()
        {
            _factory = () => new CancellationTokenSource();
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
}