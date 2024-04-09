namespace Xtuker.ConfigurationStorage;

/// <summary>
/// Конфигурация
/// </summary>
public class ConfigurationData : IConfigurationData
{
    /// <inheritdoc />
    public string Key { get; set; } = null!;

    /// <inheritdoc />
    public string? Value { get; set; }

    /// <inheritdoc />
    public bool Encrypted { get; set; }

    /// <inheritdoc />
    public void SetValue(string? value)
    {
        Value = value;
    }
}