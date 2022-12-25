using Microsoft.EntityFrameworkCore;
using Npgsql;
using TestWorker;
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
            (config, x) => x.UseAesCryptoTransformer(config))
        
        // Ef Storage
        .AddStorage((config, x) =>
        {
            var connectionString = config.GetConnectionString("Pg");
            x.UseAesCryptoTransformer(config)
                .UseEfCoreDefaultStorage(db => db.UseNpgsql(connectionString)
                        .UseLoggerFactory(loggerFactory)
                        .EnableSensitiveDataLogging(),
                    "db_config",
                    null)
                .UseEfCoreDefaultStorage(e => e.UseDbConfigurationTable("db_config", "alr", z => z.WithTableColumns("key", "value", "encrypted"))
                    .UseNpgsql(connectionString)
                    .UseLoggerFactory(loggerFactory)
                    .EnableSensitiveDataLogging());
        }))
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>()
            .AddConfigurationStorageInfrastructure();
    })
    .Build();

host.Run();