namespace Xtuker.ConfigurationStorage.EntityFramework.Extensions;

using System;
using Microsoft.EntityFrameworkCore;
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
    public static IConfigurationBuilder AddEfCoreStorage(this IConfigurationBuilder builder,
        Action<IConfiguration, DbContextOptionsBuilder> builderConfigurator,
        Action<IConfiguration, ConfigurationStorageSource>? configure = null)
    {
        var tmpConfig = builder.Build();

        var source = new ConfigurationStorageSource();
        configure?.Invoke(tmpConfig, source);

        var options = new DbContextOptionsBuilder<DefaultConfigurationDataDbContext>();
            
        builderConfigurator(tmpConfig, options);

        source.UseStorage(new EfConfigurationStorage<DefaultConfigurationDataDbContext, DefaultConfigurationDataDbContext.ConfigurationData>(new DefaultConfigurationDataDbContext(options.Options)));
            
        return builder.Add(source);
    }
        
    /// <summary>
    /// Добавить источник базы данных
    /// </summary>
    public static IConfigurationBuilder AddEfCoreStorage<TDbCtx, TConfig>(this IConfigurationBuilder builder,
        Func<DbContextOptionsBuilder<TDbCtx>, TDbCtx> factoryMethod,
        Action<IConfiguration, DbContextOptionsBuilder>? builderConfigurator = null,
        Action<IConfiguration, ConfigurationStorageSource>? configure = null)
        where TDbCtx : DbContext, IConfigurationStorageDbContext<TConfig>
        where TConfig : class, IConfigurationData
    {
        var tmpConfig = builder.Build();

        var source = new ConfigurationStorageSource();
        configure?.Invoke(tmpConfig, source);
            
        var options = new DbContextOptionsBuilder<TDbCtx>();
        builderConfigurator?.Invoke(tmpConfig, options);
            
        source.UseStorage(new EfConfigurationStorage<TDbCtx, TConfig>(factoryMethod(options)));
            
        return builder.Add(source);
    }
}