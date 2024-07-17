namespace UnitTests;

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

    public void SaveData(TestConfigurationData config)
    {
        _internalStorage[config.Key] = config with {Value = config.Value};
    }

    public string? Get(string key)
    {
        return _internalStorage.TryGetValue(key, out var value) ? value.Value : null;
    }
}