﻿namespace Xtuker.ConfigurationStorage.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

internal static class ConfigurationExtensions
{
    /// <summary>
    /// Получить экземпляр <see cref="ConfigurationStorageProvider"/>
    /// </summary>
    /// <exception cref="NotSupportedException">Провайдер не зарегистрирован</exception>
    public static ConfigurationStorageProvider GetConfigurationStorageProvider(this IConfiguration configuration)
    {
        return (configuration as IConfigurationRoot)?.Providers.OfType<ConfigurationStorageProvider>().LastOrDefault()
            ?? throw new NotSupportedException($"{nameof(ConfigurationStorageProvider)} not found");
    }

    /// <summary>
    /// Получить все экземпляры <see cref="ConfigurationStorageProvider"/>
    /// </summary>
    public static IEnumerable<ConfigurationStorageProvider> GetConfigurationStorageProviders(this IConfiguration configuration)
    {
        return (configuration as IConfigurationRoot)?.Providers.OfType<ConfigurationStorageProvider>()
            ?? ArraySegment<ConfigurationStorageProvider>.Empty;
    }
}