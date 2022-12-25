namespace Xtuker.ConfigurationStorage.EntityFramework
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public sealed class EfConfigurationStorage<TDbCtx, TConfig> : BaseConfigurationStorage<TConfig>
        where TDbCtx : DbContext
        where TConfig : class, IConfigurationData
    {
        private TDbCtx DbContext { get; }
        private DbSet<TConfig> DbSet { get; }

        public EfConfigurationStorage(TDbCtx dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TConfig>();
        }

        protected override void SetDataInternal(TConfig config)
        {
            var existRecord = DbSet.SingleOrDefault(x => x.Key == config.Key);
            if (existRecord == null)
            {
                DbSet.Add(config);
            }
            else
            {
                existRecord.Value = config.Value;
                DbSet.Update(existRecord);
            }

            DbContext.SaveChanges();
        }

        protected override IEnumerable<TConfig> GetDataInternal()
        {
            return DbSet.AsNoTracking().ToList();
        }
    }
}