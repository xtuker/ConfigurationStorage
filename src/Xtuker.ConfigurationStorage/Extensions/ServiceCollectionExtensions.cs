namespace Xtuker.ConfigurationStorage.Extensions;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xtuker.ConfigurationStorage.Crypto;

/// <summary>
/// Расширение <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Зарегистрировать используемые сервисы <see cref="IConfigurationStorage"/> в <see cref="IServiceCollection"/>
    /// </summary>
    public static IServiceCollection AddConfigurationStorageInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var storage = configuration.GetConfigurationStorageProvider().Storage
            ?? throw new NotSupportedException($"{nameof(IConfigurationStorage)} does not used");

        return serviceCollection.AddSingleton(storage).AddSingleton<IConfigurationStorageReloader, ConfigurationStorageReloader>();
    }
        
    /// <summary>
    /// Зарегистрировать используемые сервисы <see cref="IConfigurationStorage"/> в <see cref="IServiceCollection"/>
    /// </summary>
    public static IServiceCollection AddConfigurationStorageInfrastructure<TConfig>(this IServiceCollection serviceCollection, IConfiguration configuration)
        where TConfig : class, IConfigurationData
    {
        var storage = configuration.GetConfigurationStorageProvider().Storage
            ?? throw new NotSupportedException($"{nameof(IConfigurationStorage)} does not used");
        if (storage is IConfigurationStorage<TConfig> typedStorage)
        {
            serviceCollection.AddSingleton(typedStorage)
                .AddSingleton<IConfigurationStorage>(typedStorage);
        }
        else
        {
            serviceCollection.AddSingleton(storage);
        }

        if (storage.CryptoTransformer != null)
        {
            serviceCollection.AddSingleton(storage.CryptoTransformer);
        }

        return serviceCollection.AddSingleton<IConfigurationStorageReloader, ConfigurationStorageReloader>();
    }
}