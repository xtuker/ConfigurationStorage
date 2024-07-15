namespace Xtuker.ConfigurationStorage.EntityFramework.Extensions;

using System;
using Microsoft.EntityFrameworkCore;
using Xtuker.ConfigurationStorage.Extensions;

public static class ConfigurationStorageSourceExtensions
{
    /// <summary>
    /// Использовать EF Core db context
    /// </summary>
    public static ConfigurationStorageSource UseEfCoreDefaultStorage(this ConfigurationStorageSource storageSource,
        Action<DbContextOptionsBuilder> builderConfigurator,
        string tableName,
        string? schema = null,
        Action<ConfigurationDataDbContextOptions>? optionConfigurator = null)
    {
        var contextOptionsBuilder = new DbContextOptionsBuilder<DefaultConfigurationDataDbContext>();

        contextOptionsBuilder.UseDbConfigurationTable(tableName, schema, optionConfigurator);

        builderConfigurator(contextOptionsBuilder);

        storageSource.UseStorage(new EfConfigurationStorage<DefaultConfigurationDataDbContext, DefaultConfigurationDataDbContext.ConfigurationData>(new DefaultConfigurationDataDbContext(contextOptionsBuilder.Options), storageSource.CryptoTransformer));

        return storageSource;
    }

    /// <summary>
    /// Использовать EF Core db context
    /// </summary>
    /// <param name="storageSource">Источник конфигурации</param>
    /// <param name="factoryMethod">Источник конфигурации</param>
    public static ConfigurationStorageSource UseEfCoreStorage<TDbCtx, TConfig>(this ConfigurationStorageSource storageSource, Func<TDbCtx> factoryMethod)
        where TDbCtx : DbContext, IConfigurationStorageDbContext<TConfig>
        where TConfig : class, IConfigurationData
    {
        storageSource.UseStorage(new EfConfigurationStorage<TDbCtx, TConfig>(factoryMethod(), storageSource.CryptoTransformer));

        return storageSource;
    }

    /// <summary>
    /// Использовать EF Core db context
    /// </summary>
    /// <param name="storageSource">Источник конфигурации</param>
    /// <param name="factoryMethod">Источник конфигурации</param>
    public static ConfigurationStorageSource UseEfCoreStorage<TDbCtx, TConfig>(this ConfigurationStorageSource storageSource, Func<DbContextOptionsBuilder<TDbCtx>, TDbCtx> factoryMethod)
        where TDbCtx : DbContext, IConfigurationStorageDbContext<TConfig>
        where TConfig : class, IConfigurationData
    {
        var options = new DbContextOptionsBuilder<TDbCtx>();

        storageSource.UseStorage(new EfConfigurationStorage<TDbCtx, TConfig>(factoryMethod(options), storageSource.CryptoTransformer));

        return storageSource;
    }
}