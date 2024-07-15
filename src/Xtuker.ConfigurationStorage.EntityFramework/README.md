## ConfigurationStorage.EntityFramework - the .NET database configuration provider via Entity Framework Core

## Usage

```csharp
var loggerFactory = Host.CreateDefaultBuilder(args).Build().Services.GetRequiredService<ILoggerFactory>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(c => c
        .AddJsonFile("appsettings.user.json", true)
        // Ef Storage with internal DbContext
        .AddEfCoreStorage(
            // DbContext configuration
            (config, x) => x.UseDbConfigurationTable("db_config", "alr", z => z.WithTableColumns("key", "value", "encrypted"))
                .UseNpgsql(config.GetConnectionString("Pg"))
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging(),
            // ConfigurationStorage configuration
            (config, x) => x.UseAesCryptoTransformer(config)
                .UseLoggerFactory(loggerFactory)
                .ReloadOnExpiry()
        )
        // or
        // Ef Storage with user DbContext
        .AddEfCoreStorage<MyDbContext, MyConfigurationData>(
            // DbContext factory
            ops => new MyDbContext(ops.Options),
            // DbContext configuration
            (config, x) => x.UseNpgsql(config.GetConnectionString("Pg"))
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging(),
            // ConfigurationStorage configuration
            (config, x) => x.UseAesCryptoTransformer(config)
                .UseLoggerFactory(loggerFactory)
                .ReloadOnExpiry()
        )
    )
    .ConfigureServices((ctx, services) =>
    {
        // optional register services if used
        // IConfigurationStorage,
        // IConfigurationStorage<TConfig>,
        // IConfigurationCryptoTransformer,
        // IConfigurationStorageReloader
        services.AddConfigurationStorageInfrastructure(ctx.Configuration);
    })
    .Build();

host.Run();
```