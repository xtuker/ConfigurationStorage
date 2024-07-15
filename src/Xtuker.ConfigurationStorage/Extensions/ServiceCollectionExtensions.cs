namespace Xtuker.ConfigurationStorage.Extensions;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        foreach (var storageType in storage.GetType().GetInterfaces())
        {
            serviceCollection.AddSingleton(storageType, storage);
        }

        if (storage.CryptoTransformer != null)
        {
            serviceCollection.AddSingleton(storage.CryptoTransformer);
        }

        return serviceCollection.AddSingleton<IConfigurationStorageReloader, ConfigurationStorageReloader>();
    }
}