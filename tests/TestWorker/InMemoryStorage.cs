namespace TestWorker;

using Xtuker.ConfigurationStorage;

internal class InMemoryStorage : IConfigurationStorage
{
    private readonly IDictionary<string, MyConfigurationData> _data = new Dictionary<string, MyConfigurationData>()
    {
        ["Test:Value"] = new MyConfigurationData
        {
            Key = "Test:Value",
            Value = $"Value from {nameof(InMemoryStorage)}",
            Encrypted = false
        }
    };

    public void SetData(MyConfigurationData configurationData)
    {
        _data[configurationData.Key] = configurationData;
    }

    public IEnumerable<IConfigurationData> GetData()
    {
        return _data.Values;
    }
}