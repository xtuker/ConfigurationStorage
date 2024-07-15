namespace TestWorker.ef;

using Microsoft.EntityFrameworkCore;
using Xtuker.ConfigurationStorage.EntityFramework;

internal sealed class MyDbContext : DbContext, IConfigurationStorageDbContext<MyConfigurationData>
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MyConfigurationDataMapping());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<MyConfigurationData> ConfigurationDataDbSet { get; set; }
}