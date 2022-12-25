namespace Xtuker.ConfigurationStorage.Extensions
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Расширение <see cref="IConfigurationBuilder"/>
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Добавить источник базы данных
        /// </summary>
        public static IConfigurationBuilder AddStorage(this IConfigurationBuilder builder, Action<IConfiguration, ConfigurationStorageSource> configure)
        {
            var tmpConfig = builder.Build();
            
            var source = new ConfigurationStorageSource();
            configure(tmpConfig, source);
            return builder.Add(source);
        }
        
        /// <summary>
        /// Добавить источник базы данных
        /// </summary>
        public static IConfigurationBuilder AddStorage(this IConfigurationBuilder builder, Action<ConfigurationStorageSource> configure)
        {
            var source = new ConfigurationStorageSource();
            configure(source);
            return builder.Add(source);
        }
    }
}