namespace TestWorker;

using Xtuker.ConfigurationStorage;

public class InMemoryStorage : BaseConfigurationStorage<ConfigurationData>
{
    private readonly IDictionary<string, ConfigurationData> _data = new Dictionary<string, ConfigurationData>()
    {
        ["Test:Value"] = new ConfigurationData
        {
            Key = "Test:Value",
            Value = $"Value from {nameof(InMemoryStorage)}",
            Encrypted = false
        }
    };

    protected override void SetDataInternal(ConfigurationData configurationData)
    {
        _data[configurationData.Key] = configurationData;
    }

    protected override IEnumerable<ConfigurationData> GetDataInternal()
    {
        return _data.Values;
    }
}