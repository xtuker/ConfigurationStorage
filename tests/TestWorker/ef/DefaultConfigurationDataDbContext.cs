namespace TestWorker.ef
{
    using Microsoft.EntityFrameworkCore;

    internal sealed class MyDbContext : DbContext
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
    }
}