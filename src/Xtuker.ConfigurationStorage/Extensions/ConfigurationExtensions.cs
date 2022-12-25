namespace Xtuker.ConfigurationStorage.Extensions
{
    using System;
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
            return (configuration as IConfigurationRoot)?.Providers.LastOrDefault(x => x is ConfigurationStorageProvider) as ConfigurationStorageProvider
                ?? throw new NotSupportedException($"{nameof(ConfigurationStorageProvider)} not found");
        }
    }
}