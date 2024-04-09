namespace Xtuker.ConfigurationStorage.EntityFramework.Extensions;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

public static class DbContextOptionsBuilderExtensions
{
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