namespace Xtuker.ConfigurationStorage.Dapper.Extensions;

using System;
using System.Data;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Extensions for <see cref="IConfigurationBuilder"/>
/// </summary>
public static class ConfigurationBuilderExtensions
{

    /// <summary>
    /// Add storage with dapper orm
    /// </summary>
    /// <param name="builder">configuration builder</param>
    /// <param name="connectionStringFactory">connection string factory</param>
    /// <param name="sqlCommand">sql command for get all configuration data</param>
    /// <param name="dbConnectionFactory"><see cref="IDbConnection"/> factory</param>
    /// <param name="configure">configuration storage configurator</param>
    /// <typeparam name="TConfig">configuration model type</typeparam>
    public static IConfigurationBuilder AddDapperStorage<TConfig>(this IConfigurationBuilder builder,
        Func<IConfiguration, string> connectionStringFactory,
        string sqlCommand,
        Func<string, IDbConnection> dbConnectionFactory,
        Action<IConfiguration, ConfigurationStorageSource>? configure = null)
        where TConfig: class, IConfigurationData
    {
        var tmpConfig = builder.Build();
        var connectionString = connectionStringFactory(tmpConfig);
            
        var source = new ConfigurationStorageSource(new DapperConfigurationStorage<TConfig>(connectionString, sqlCommand, dbConnectionFactory));
        configure?.Invoke(tmpConfig, source);
            
        return builder.Add(source);
    }

    /// <summary>
    /// Add storage with dapper orm
    /// </summary>
    /// <param name="builder">configuration builder</param>
    /// <param name="connectionStringFactory">connection string factory</param>
    /// <param name="sqlCommand">sql command for get all configuration data</param>
    /// <param name="dbConnectionFactory"><see cref="IDbConnection"/> factory</param>
    /// <param name="configure">configuration storage configurator</param>
    public static IConfigurationBuilder AddDapperStorage(this IConfigurationBuilder builder,
        Func<IConfiguration, string> connectionStringFactory,
        string sqlCommand,
        Func<string, IDbConnection> dbConnectionFactory,
        Action<IConfiguration, ConfigurationStorageSource>? configure = null)
    {
        var tmpConfig = builder.Build();
        var connectionString = connectionStringFactory(tmpConfig);

        var source = new ConfigurationStorageSource(new DapperConfigurationStorage(connectionString, sqlCommand, dbConnectionFactory));
        configure?.Invoke(tmpConfig, source);

        return builder.Add(source);
    }
}