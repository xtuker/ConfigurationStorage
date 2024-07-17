namespace Xtuker.ConfigurationStorage;

using Xtuker.ConfigurationStorage.Crypto;

/// <summary>
/// Key/Value configuration
/// </summary>
public interface IConfigurationData
{
    /// <summary>
    /// Key
    /// <example>Foo:Bar:Baz</example>
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Value
    /// </summary>
    string? Value { get; set; }
        
    /// <summary>
    /// If <see langword="true"/>, <see cref="Value"/> must be encrypted in storage
    /// </summary>
    /// <seealso cref="IConfigurationCryptoTransformer"/>
    bool Encrypted { get; }
}