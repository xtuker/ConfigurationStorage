namespace TestWorker;

using Xtuker.ConfigurationStorage;

internal class InMemoryStorage : BaseConfigurationStorage<MyConfigurationData>
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

    protected override void SetDataInternal(MyConfigurationData configurationData)
    {
        _data[configurationData.Key] = configurationData;
    }

    protected override IEnumerable<IConfigurationData> GetDataInternal()
    {
        return _data.Values;
    }

    public InMemoryStorage()
        : base(null)
    {
    }
}