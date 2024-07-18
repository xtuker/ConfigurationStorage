## ConfigurationStorage.EntityFramework - the .NET configuration provider from database via Entity Framework Core

## Usage

```csharp
var loggerFactory = Host.CreateDefaultBuilder(args).Build().Services.GetRequiredService<ILoggerFactory>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(c => c
        .AddJsonFile("appsettings.user.json", true)
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
        // optional register services if used
        // IConfigurationStorageReloader,
        // IConfigurationStorageChangeNotificationService,
        // IConfigurationCryptoTransformer,
        services.AddConfigurationStorageInfrastructure(ctx.Configuration);
    })
    .Build();

host.Run();
```