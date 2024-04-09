namespace Xtuker.ConfigurationStorage.EntityFramework;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public abstract class BaseConfigurationDataMapping<TConfig> : IEntityTypeConfiguration<TConfig>
    where TConfig : class, IConfigurationData
{
    private readonly string _tableName;
    private readonly string? _schemaName;

    protected virtual string KeyColumnName { get; } = "Key";
    protected virtual string ValueColumnName { get; } = "Value";
    protected virtual string EncryptedColumnName { get; } = "Encrypted";

    protected BaseConfigurationDataMapping(string tableName, string? schemaName = null)
    {
        _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        _schemaName = schemaName;
    }

    protected virtual void ConfigureOther(EntityTypeBuilder<TConfig> builder)
    {
    }

    public void Configure(EntityTypeBuilder<TConfig> builder)
    {
        builder.ToTable(_tableName, _schemaName);
        builder.HasIndex(x => x.Key).IsUnique();

        builder.Property(x => x.Key).HasColumnName(KeyColumnName).IsRequired();
        builder.Property(x => x.Value).HasColumnName(ValueColumnName);
        builder.Property(x => x.Encrypted).HasColumnName(EncryptedColumnName).IsRequired();

        ConfigureOther(builder);
    }
}