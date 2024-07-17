namespace Xtuker.ConfigurationStorage;

using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

/// <summary>
/// <see cref="IConfigurationData"/> storage for <see cref="IConfigurationProvider"/>
/// </summary>
public interface IConfigurationStorage
{
    /// <summary>
    /// Get configuration data
    /// </summary>
    IEnumerable<IConfigurationData> GetData();
}