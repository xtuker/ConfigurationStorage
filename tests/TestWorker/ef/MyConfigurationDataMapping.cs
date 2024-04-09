namespace TestWorker.ef;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xtuker.ConfigurationStorage;

public class MyConfigurationDataMapping : IEntityTypeConfiguration<ConfigurationData>
{
    public void Configure(EntityTypeBuilder<ConfigurationData> builder)
    {
        builder.ToTable("my_configuration");
        builder.HasIndex(x => x.Key).IsUnique();

        builder.Property(x => x.Key).HasColumnName("key").IsRequired();
        builder.Property(x => x.Value).HasColumnName("value");
        builder.Property(x => x.Encrypted).HasColumnName("is_encrypted").IsRequired();
    }
}