namespace Xtuker.ConfigurationStorage.EntityFramework;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

internal sealed class EfConfigurationStorage<TDbCtx, TConfig> : IConfigurationStorage
    where TDbCtx : DbContext, IConfigurationStorageDbContext<TConfig>
    where TConfig : class, IConfigurationData
{
    private TDbCtx DbContext { get; }

    public EfConfigurationStorage(TDbCtx dbContext)
    {
        DbContext = dbContext;
    }

    public IEnumerable<IConfigurationData> GetData()
    {
        return DbContext.ConfigurationDataDbSet.AsNoTracking().ToList();
    }
}