namespace UnitTests;

using Xtuker.ConfigurationStorage;

internal record TestConfigurationData : IConfigurationData
{
    public required string Key { get; init; }

    public string? Value { get; set; }

    public bool Encrypted { get; init; }

    public override string ToString()
    {
        return $"{Key}={Value}";
    }
}