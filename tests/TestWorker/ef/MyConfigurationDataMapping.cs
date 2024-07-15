namespace TestWorker.ef;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class MyConfigurationDataMapping : IEntityTypeConfiguration<MyConfigurationData>
{
    public void Configure(EntityTypeBuilder<MyConfigurationData> builder)
    {
        builder.ToTable("my_configuration");
        builder.HasKey(x => x.Key);

        builder.Property(x => x.Key).HasColumnName("key").IsRequired();
        builder.Property(x => x.Value).HasColumnName("value");
        builder.Property(x => x.Encrypted).HasColumnName("is_encrypted").IsRequired();
    }
}