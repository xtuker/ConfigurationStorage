namespace Xtuker.ConfigurationStorage.Extensions;

using System;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Extensions for <see cref="IConfigurationBuilder"/>
/// </summary>
public static class ConfigurationBuilderExtensions
{
    /// <summary>
    /// Add configuration storage
    /// </summary>
    public static IConfigurationBuilder AddStorage(this IConfigurationBuilder builder,
        IConfigurationStorage storage,
        Action<IConfiguration, ConfigurationStorageSource> configure)
    {
        var tmpConfig = builder.Build();

        var source = new ConfigurationStorageSource(storage);
        configure(tmpConfig, source);
        return builder.Add(source);
    }
        
    /// <summary>
    /// Add configuration storage
    /// </summary>
    public static IConfigurationBuilder AddStorage(this IConfigurationBuilder builder,
        IConfigurationStorage storage,
        Action<ConfigurationStorageSource> configure)
    {
        var source = new ConfigurationStorageSource(storage);
        configure(source);
        return builder.Add(source);
    }
}