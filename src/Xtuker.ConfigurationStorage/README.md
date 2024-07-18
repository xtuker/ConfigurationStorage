# ConfigurationStorage - the .NET configuration provider from database

## Usage

```csharp
var loggerFactory = Host.CreateDefaultBuilder(args).Build().Services.GetRequiredService<ILoggerFactory>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(c => c
        .AddJsonFile("appsettings.user.json", true)
        // Custom Storage
        .AddStorage(new MyConfigurationStorage(new MyCryptoTransformer()),
            c =>
            {
                c.UseAesCryptoTransformer(key)
                    .UseLoggerFactory(loggerFactory)
                    .ReloadOnExpiry(120);
            })
    )
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
```