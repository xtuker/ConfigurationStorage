namespace Xtuker.ConfigurationStorage.EntityFramework
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal sealed class DefaultConfigurationDataDbContext : DbContext
    {
        private readonly ConfigurationDataDbContextOptions _ops;
        public DefaultConfigurationDataDbContext(DbContextOptions<DefaultConfigurationDataDbContext> options)
            : base(options)
        {
            _ops = options.GetExtension<ConfigurationDataDbContextOptions>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigurationDataMapping(_ops));

            base.OnModelCreating(modelBuilder);
        }

        private class ConfigurationDataMapping : BaseConfigurationDataMapping<ConfigurationData>
        {
            protected override string KeyColumnName { get; }
            protected override string ValueColumnName { get; }

            public ConfigurationDataMapping(ConfigurationDataDbContextOptions options)
                : base(options.TableName, options.SchemaName)
            {
                KeyColumnName = options.KeyColumnName;
                ValueColumnName = options.ValueColumnName;
            }

            protected override void ConfigureOther(EntityTypeBuilder<ConfigurationData> builder)
            {
                base.ConfigureOther(builder);
                
                builder.HasKey(x => x.Key);
            }
        }
    }
}