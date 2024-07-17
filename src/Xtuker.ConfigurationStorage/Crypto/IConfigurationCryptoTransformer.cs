namespace Xtuker.ConfigurationStorage.Crypto;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Encrypt/Decrypt service for <see cref="IConfigurationData"/>
/// </summary>
public interface IConfigurationCryptoTransformer
{
    /// <summary>
    /// Encrypt <see cref="IConfigurationData.Value"/> if <see cref="IConfigurationData.Encrypted"/> is <see langword="true"/>
    /// </summary>
    /// <param name="configurationData">configuration value instance</param>
    /// <param name="silent">suppress exceptions</param>
    [return: NotNullIfNotNull(nameof(configurationData))]
    public T? Encrypt<T>(T? configurationData, bool silent = true)
        where T : IConfigurationData;

    /// <summary>
    /// Decrypt <see cref="IConfigurationData.Value"/> if <see cref="IConfigurationData.Encrypted"/> is <see langword="true"/>
    /// </summary>
    /// <param name="configurationData">configuration value instance</param>
    /// <param name="silent">suppress exceptions</param>
    [return: NotNullIfNotNull(nameof(configurationData))]
    public T? Decrypt<T>(T? configurationData, bool silent = true)
        where T : IConfigurationData;
        
    /// <summary>
    /// Encrypt each <see cref="IConfigurationData.Value"/> if <see cref="IConfigurationData.Encrypted"/> is <see langword="true"/>
    /// </summary>
    /// <param name="configurationDatas">configuration values</param>
    /// <param name="silent">suppress exceptions</param>
    public IEnumerable<T> Encrypt<T>(IEnumerable<T> configurationDatas, bool silent = true)
        where T : class, IConfigurationData;
        
    /// <summary>
    /// Decrypt each <see cref="IConfigurationData.Value"/> if <see cref="IConfigurationData.Encrypted"/> is <see langword="true"/>
    /// </summary>
    /// <param name="configurationDatas">configuration values</param>
    /// <param name="silent">suppress exceptions</param>
    public IEnumerable<T> Decrypt<T>(IEnumerable<T> configurationDatas, bool silent = true)
        where T : class, IConfigurationData;
}