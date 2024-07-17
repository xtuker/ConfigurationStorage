namespace Xtuker.ConfigurationStorage.EntityFramework.Extensions;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

/// <summary>
/// Extensions for <see cref="DbContextOptionsBuilder"/>
/// </summary>
public static class DbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Use <see cref="ConfigurationDataDbContextOptions"/> db context extension
    /// </summary>
    /// <param name="optionsBuilder">db context configurator</param>
    /// <param name="tableName">configuration storage table name</param>
    /// <param name="schemaName">configuration storage schema name</param>
    /// <param name="configurator">db context extension configurator</param>
    /// <returns></returns>
    public static DbContextOptionsBuilder UseDbConfigurationTable(this DbContextOptionsBuilder optionsBuilder, string tableName, string? schemaName = null, Action<ConfigurationDataDbContextOptions>? configurator = null)
    {
        var extension = (optionsBuilder.Options.FindExtension<ConfigurationDataDbContextOptions>()
                ?? new ConfigurationDataDbContextOptions())
            .WithTableName(tableName);

        if (schemaName != null)
        {
            extension = extension.WithSchemaName(schemaName);
        }
            
        configurator?.Invoke(extension);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}