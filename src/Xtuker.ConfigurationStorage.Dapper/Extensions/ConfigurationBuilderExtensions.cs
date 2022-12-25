namespace Xtuker.ConfigurationStorage.Dapper.Extensions
{
    using System;
    using System.Data;
    using Microsoft.Extensions.Configuration;
    using Xtuker.ConfigurationStorage.Extensions;

    /// <summary>
    /// Расширение <see cref="IConfigurationBuilder"/>
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Добавить источник базы данных
        /// </summary>
        public static IConfigurationBuilder AddDapperStorage(this IConfigurationBuilder builder,
            Func<IConfiguration, string> connectionStringConfigurator,
            string sqlCommand,
            Func<string, IDbConnection> dbConnectionFactory,
            Action<IConfiguration, ConfigurationStorageSource>? configure = null)
        {
            var tmpConfig = builder.Build();
            var connectionString = connectionStringConfigurator(tmpConfig);
            
            var source = new ConfigurationStorageSource();
            configure?.Invoke(tmpConfig, source);

            source.UseStorage(new DapperConfigurationReadOnlyStorage(connectionString, sqlCommand, dbConnectionFactory, source.CryptoTransformer));
            
            return builder.Add(source);
        }
    }
}