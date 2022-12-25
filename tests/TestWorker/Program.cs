using Microsoft.EntityFrameworkCore;
using Npgsql;
using TestWorker;
using TestWorker.ef;
using Xtuker.ConfigurationStorage;
using Xtuker.ConfigurationStorage.Dapper.Extensions;
using Xtuker.ConfigurationStorage.EntityFramework.Extensions;
using Xtuker.ConfigurationStorage.Extensions;

var loggerFactory = Host.CreateDefaultBuilder(args).Build().Services.GetRequiredService<ILoggerFactory>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(c => c
        .AddJsonFile("appsettings.user.json", true)
        // Dapper Storage
        .AddDapperStorage(config => config.GetConnectionString("Pg")!,
            "SELECT * FROM alr.db_config",
            connectionString => new NpgsqlConnection(connectionString),
            (config, x) => x.UseAesCryptoTransformer(config).UseLoggerFactory(loggerFactory).ReloadOnExpiry())

        // Ef Storage
        .AddEfCoreStorage((config, x) => x.UseDbConfigurationTable("db_config", "alr", z => z.WithTableColumns("key", "value", "encrypted"))
                .UseNpgsql(config.GetConnectionString("Pg"))
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging(),
            (config, x) => x.UseAesCryptoTransformer(config).UseLoggerFactory(loggerFactory).ReloadOnExpiry())

        // Ef Storage
        .AddEfCoreStorage<MyDbContext, ConfigurationCryptoData>(ops => new MyDbContext(ops.Options),
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