## ConfigurationStorage.Dapper - the .NET configuration provider from database via Dapper

## Usage

```csharp
var loggerFactory = Host.CreateDefaultBuilder(args).Build().Services.GetRequiredService<ILoggerFactory>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(c => c
        .AddJsonFile("appsettings.user.json", true)
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