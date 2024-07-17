namespace Xtuker.ConfigurationStorage.EntityFramework.Extensions;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Extensions for <see cref="IConfigurationBuilder"/>
/// </summary>
public static class ConfigurationBuilderExtensions
{
    /// <summary>
    /// Add storage with entity framework orm
    /// </summary>
    /// <param name="builder">configuration builder</param>
    /// <param name="builderConfigurator"><see cref="DbContext"/> configurator</param>
    /// <param name="configure">configuration storage configurator</param>
    public static IConfigurationBuilder AddEfCoreStorage(this IConfigurationBuilder builder,
        Action<IConfiguration, DbContextOptionsBuilder> builderConfigurator,
        Action<IConfiguration, ConfigurationStorageSource>? configure = null)
    {
        var tmpConfig = builder.Build();

        var options = new DbContextOptionsBuilder<DefaultConfigurationDataDbContext>();
        builderConfigurator(tmpConfig, options);
        var storage =
            new EfConfigurationStorage<DefaultConfigurationDataDbContext, DefaultConfigurationDataDbContext.ConfigurationData>(
                new DefaultConfigurationDataDbContext(options.Options));

        var source = new ConfigurationStorageSource(storage);
        configure?.Invoke(tmpConfig, source);
            
        return builder.Add(source);
    }
        
    /// <summary>
    /// Add storage with entity framework orm
    /// </summary>
    /// <param name="builder">configuration builder</param>
    /// <param name="factoryMethod"><see cref="DbContext"/> factory</param>
    /// <param name="builderConfigurator"><see cref="DbContext"/> configurator</param>
    /// <param name="configure">configuration storage configurator</param>
    /// <typeparam name="TDbCtx"><see cref="DbContext"/> type</typeparam>
    /// <typeparam name="TConfig">configuration model type</typeparam>
    public static IConfigurationBuilder AddEfCoreStorage<TDbCtx, TConfig>(this IConfigurationBuilder builder,
        Func<DbContextOptionsBuilder<TDbCtx>, TDbCtx> factoryMethod,
        Action<IConfiguration, DbContextOptionsBuilder>? builderConfigurator = null,
        Action<IConfiguration, ConfigurationStorageSource>? configure = null)
        where TDbCtx : DbContext, IConfigurationStorageDbContext<TConfig>
        where TConfig : class, IConfigurationData
    {
        var tmpConfig = builder.Build();

        var options = new DbContextOptionsBuilder<TDbCtx>();
        builderConfigurator?.Invoke(tmpConfig, options);

        var source = new ConfigurationStorageSource(new EfConfigurationStorage<TDbCtx, TConfig>(factoryMethod(options)));
        configure?.Invoke(tmpConfig, source);

        return builder.Add(source);
    }
}