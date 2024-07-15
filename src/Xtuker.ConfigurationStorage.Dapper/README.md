## ConfigurationStorage.Dapper - the .NET database configuration provider via Dapper

## Usage

```csharp
var loggerFactory = Host.CreateDefaultBuilder(args).Build().Services.GetRequiredService<ILoggerFactory>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(c => c
        .AddJsonFile("appsettings.user.json", true)
        // Dapper Storage with internal model
        .AddDapperStorage(
            config => config.GetConnectionString("Pg")!,
            // Dapper Storage
            $@"SELECT
                ""{nameof(IConfigurationData.Key)}"",
                ""{nameof(IConfigurationData.Value)}"",
                ""{nameof(IConfigurationData.Encrypted)}""
              FROM config_table_name",
            connectionString => new NpgsqlConnection(connectionString),
            (config, x) => x.UseAesCryptoTransformer(config)
                .UseLoggerFactory(loggerFactory)
                .ReloadOnExpiry()
        )
    
        // Dapper Storage with user model
        .AddDapperStorage<MyConfigurationData>(
            config => config.GetConnectionString("Pg")!,
            // Dapper Storage
            $@"SELECT * FROM config_table_name",
            connectionString => new NpgsqlConnection(connectionString),
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