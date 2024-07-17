namespace Xtuker.ConfigurationStorage;

using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Xtuker.ConfigurationStorage.Extensions;

/// <inheritdoc />
internal class ConfigurationStorageReloader : IConfigurationStorageReloader
{
    private readonly ICollection<ConfigurationStorageProvider> _storageProvider;

    public ConfigurationStorageReloader(IConfiguration configuration)
    {
        _storageProvider = configuration.GetConfigurationStorageProviders().ToList();
    }

    /// <inheritdoc />
    public void Reload()
    {
        foreach (var storageProvider in _storageProvider)
        {
            storageProvider.Reload();
        }
    }
}