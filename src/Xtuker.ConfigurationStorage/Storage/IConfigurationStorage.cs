namespace Xtuker.ConfigurationStorage;

using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Хранилище данных конфигурации <see cref="IConfigurationProvider"/>
/// </summary>
public interface IConfigurationStorage
{
    /// <summary>
    /// Получить набор данных из хранилища
    /// </summary>
    IEnumerable<IConfigurationData> GetData();
}