namespace TestWorker;

using Xtuker.ConfigurationStorage;

internal class MyConfigurationData : IConfigurationData
{
    public required string Key { get; init; }

    public string? Value { get; set; }

    public bool Encrypted { get; init; }
}