namespace Xtuker.ConfigurationStorage.EntityFramework;

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xtuker.ConfigurationStorage.Crypto;

internal sealed class EfConfigurationStorage<TDbCtx, TConfig> : BaseConfigurationStorage<TConfig>
    where TDbCtx : DbContext, IConfigurationStorageDbContext<TConfig>
    where TConfig : class, IConfigurationData
{
    private TDbCtx DbContext { get; }

    public EfConfigurationStorage(TDbCtx dbContext, IConfigurationCryptoTransformer? cryptoTransformer)
        : base(cryptoTransformer)
    {
        DbContext = dbContext;
    }

    protected override void SetDataInternal(TConfig config)
    {
        var existRecord = DbContext.ConfigurationDataDbSet.SingleOrDefault(x => x.Key == config.Key);
        if (existRecord == null)
        {
            DbContext.ConfigurationDataDbSet.Add(config);
        }
        else
        {
            existRecord.Value = config.Value;
            DbContext.ConfigurationDataDbSet.Update(existRecord);
        }

        DbContext.SaveChanges();
    }

    protected override IEnumerable<IConfigurationData> GetDataInternal()
    {
        return DbContext.ConfigurationDataDbSet.AsNoTracking().ToList();
    }
}