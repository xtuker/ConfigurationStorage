using Microsoft.EntityFrameworkCore;
using Npgsql;
using TestWorker;
using TestWorker.ef;
using Xtuker.ConfigurationStorage;
using Xtuker.ConfigurationStorage.Crypto;
using Xtuker.ConfigurationStorage.Dapper.Extensions;
using Xtuker.ConfigurationStorage.EntityFramework.Extensions;
using Xtuker.ConfigurationStorage.Extensions;

var loggerFactory = Host.CreateDefaultBuilder(args).Build().Services.GetRequiredService<ILoggerFactory>();


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(c => c
        .AddJsonFile("appsettings.user.json", true)

        // Custom Storage
        .AddStorage(x =>
        {
            x.UseStorage(new MyConfigurationStorage(new MyCryptoTransformer()))
                .UseLoggerFactory(loggerFactory)
                .ReloadOnExpiry(120);

        })
        // Dapper Storage
        .AddDapperStorage(config => config.GetConnectionString("Pg")!,
            $@"SELECT ""{nameof(IConfigurationData.Key)}"", ""{nameof(IConfigurationData.Value)}"", ""{nameof(IConfigurationData.Encrypted)}"" FROM alr.db_config",
            connectionString => new NpgsqlConnection(connectionString),
            (config, x) => x.UseAesCryptoTransformer(config).UseLoggerFactory(loggerFactory).ReloadOnExpiry())

        .AddDapperStorage<MyConfigurationData>(config => config.GetConnectionString("Pg")!,
            $@"SELECT ""{nameof(IConfigurationData.Key)}"", ""{nameof(IConfigurationData.Value)}"", ""{nameof(IConfigurationData.Encrypted)}"" FROM alr.db_config",
            connectionString => new NpgsqlConnection(connectionString),
            (config, x) => x.UseAesCryptoTransformer(config).UseLoggerFactory(loggerFactory).ReloadOnExpiry())

        // Ef Storage
        .AddEfCoreStorage(
            (config, x) => x.UseDbConfigurationTable("db_config", "alr", z => z.WithTableColumns("key", "value", "encrypted"))
                .UseNpgsql(config.GetConnectionString("Pg"))
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging(),
            (config, x) => x.UseAesCryptoTransformer(config).UseLoggerFactory(loggerFactory).ReloadOnExpiry())

        // Ef Storage
        .AddEfCoreStorage<MyDbContext, MyConfigurationData>(ops => new MyDbContext(ops.Options),
            (config, x) => x.UseNpgsql(config.GetConnectionString("Pg"))
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging(),
            (config, x) => x.UseAesCryptoTransformer(config).UseLoggerFactory(loggerFactory).ReloadOnExpiry())
    )
    .ConfigureServices((ctx, services) =>
    {
        services.AddHostedService<Worker>()
            .AddConfigurationStorageInfrastructure(ctx.Configuration);
    })
    .Build();

host.Run();


class MyConfigurationStorage : IConfigurationStorage
{
    public MyConfigurationStorage(IConfigurationCryptoTransformer? cryptoTransformer)
    {
        CryptoTransformer = cryptoTransformer;
    }

    public IConfigurationCryptoTransformer? CryptoTransformer { get; }

    public IEnumerable<IConfigurationData> GetData()
    {
        throw new NotImplementedException();
    }
}

class MyCryptoTransformer : IConfigurationCryptoTransformer
{
    public T? Encrypt<T>(T? configurationData, bool silent = true)
        where T : IConfigurationData
    {
        throw new NotImplementedException();
    }

    public T? Decrypt<T>(T? configurationData, bool silent = true)
        where T : IConfigurationData
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> Encrypt<T>(IEnumerable<T> configurationDatas, bool silent = true)
        where T : class, IConfigurationData
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> Decrypt<T>(IEnumerable<T> configurationDatas, bool silent = true)
        where T : class, IConfigurationData
    {
        throw new NotImplementedException();
    }
}