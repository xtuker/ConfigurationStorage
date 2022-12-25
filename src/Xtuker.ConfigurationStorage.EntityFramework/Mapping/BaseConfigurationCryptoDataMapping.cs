namespace Xtuker.ConfigurationStorage.EntityFramework
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public abstract class BaseConfigurationCryptoDataMapping<TConfig> : BaseConfigurationDataMapping<TConfig>
        where TConfig : class, IConfigurationData
    {
        protected virtual string EncryptedColumnName { get; } = "Encrypted";

        protected BaseConfigurationCryptoDataMapping(string tableName, string? schemaName = null)
            : base(tableName, schemaName)
        {
        }

        protected override void ConfigureOther(EntityTypeBuilder<TConfig> builder)
        {
            base.ConfigureOther(builder);
            
            builder.Property(x => x.Encrypted).HasColumnName(EncryptedColumnName).IsRequired();
        }
    }
}