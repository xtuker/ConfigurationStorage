namespace Xtuker.ConfigurationStorage.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

internal static class ConfigurationExtensions
{
    /// <summary>
    /// Get <see cref="ConfigurationStorageProvider"/> instance
    /// </summary>
    /// <exception cref="NotSupportedException">provider does not found</exception>
    public static ConfigurationStorageProvider GetConfigurationStorageProvider(this IConfiguration configuration)
    {
        return (configuration as IConfigurationRoot)?.Providers.OfType<ConfigurationStorageProvider>().LastOrDefault()
            ?? throw new NotSupportedException($"{nameof(ConfigurationStorageProvider)} does not found");
    }

    /// <summary>
    /// Get all <see cref="ConfigurationStorageProvider"/> instances
    /// </summary>
    public static IEnumerable<ConfigurationStorageProvider> GetConfigurationStorageProviders(this IConfiguration configuration)
    {
        return (configuration as IConfigurationRoot)?.Providers.OfType<ConfigurationStorageProvider>()
            ?? ArraySegment<ConfigurationStorageProvider>.Empty;
    }
}