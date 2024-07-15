namespace Xtuker.ConfigurationStorage.EntityFramework;

using Microsoft.EntityFrameworkCore;

internal sealed class DefaultConfigurationDataDbContext : DbContext, IConfigurationStorageDbContext<DefaultConfigurationDataDbContext.ConfigurationData>
{
    private readonly ConfigurationDataDbContextOptions _ops;

    public DbSet<ConfigurationData> ConfigurationDataDbSet { get; set; }

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
        protected override string EncryptedColumnName { get; }

        public ConfigurationDataMapping(ConfigurationDataDbContextOptions options)
            : base(options.TableName, options.SchemaName)
        {
            KeyColumnName = options.KeyColumnName;
            ValueColumnName = options.ValueColumnName;
            EncryptedColumnName = options.EncryptedColumnName;
        }
    }

    internal class ConfigurationData : IConfigurationData
    {
        public string Key { get; set; } = null!;

        public string? Value { get; set; }

        public bool Encrypted { get; set; }
    }
}