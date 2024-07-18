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
        .AddStorage(new MyConfigurationStorage(new MyCryptoTransformer()),
            x =>
            {
                x.UseLoggerFactory(loggerFactory)
                    .ReloadOnExpiry(120);
            })
        // Dapper Storage
        .AddDapperStorage(
            // connection string factory
            config => config.GetConnectionString("Pg")!,
            // get SQL query
            $@"SELECT 
                ""{nameof(IConfigurationData.Key)}"",
                ""{nameof(IConfigurationData.Value)}"",
                ""{nameof(IConfigurationData.Encrypted)}""
              FROM db_config_schema.db_config_table",
            // db connection factory
            connectionString => new NpgsqlConnection(connectionString),
            // configure storage
            (config, x) => x.UseAesCryptoTransformer(config)
                .UseLoggerFactory(loggerFactory)
                .ReloadOnExpiry()
        )

        .AddDapperStorage<MyConfigurationData>(
            // connection string factory
            config => config.GetConnectionString("Pg")!,
            // get SQL query
            "SELECT * FROM db_config_schema.db_config_table",
            // db connection factory
            connectionString => new NpgsqlConnection(connectionString),
            // configure storage
            (config, x) => x.UseAesCryptoTransformer(config)
                .UseLoggerFactory(loggerFactory)
                .ReloadOnExpiry()
        )

        // Ef Storage with internal DbContext
        .AddEfCoreStorage(
            // configure db context
            (config, x) =>
                x.UseDbConfigurationTable(
                    "db_config_table",
                    "db_config_schema",
                    z => z.WithTableColumns("key", "value", "encrypted")
                )
                .UseNpgsql(config.GetConnectionString("Pg"))
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging(),
            // configure storage
            (config, x) => x.UseAesCryptoTransformer(config)
                .UseLoggerFactory(loggerFactory)
                .ReloadOnExpiry()
        )

        // Ef Storage with user DbContext implemented IConfigurationStorageDbContext<T>
        .AddEfCoreStorage<MyDbContext, MyConfigurationData>(
            // db context factory
            ops => new MyDbContext(ops.Options),
            // configure db context
            (config, x) => x.UseNpgsql(config.GetConnectionString("Pg"))
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging(),
            // configure storage
            (config, x) => x.UseAesCryptoTransformer(config)
                .UseLoggerFactory(loggerFactory)
                .ReloadOnExpiry()
        )
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