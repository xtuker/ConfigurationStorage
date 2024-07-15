namespace Xtuker.ConfigurationStorage.EntityFramework;

using Microsoft.EntityFrameworkCore;

public interface IConfigurationStorageDbContext<TConfig>
    where TConfig: class, IConfigurationData
{
    DbSet<TConfig> ConfigurationDataDbSet { get; }
}