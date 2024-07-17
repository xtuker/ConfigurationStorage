namespace Xtuker.ConfigurationStorage.Extensions;

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
    /// <remarks>
    /// <see cref="IConfigurationStorageReloader"/><br/>
    /// <see cref="IConfigurationStorageChangeNotifier"/>
    /// <see cref="IConfigurationCryptoTransformer"/> if configured<br/>
    /// </remarks>
    /// </summary>
    public static IServiceCollection AddConfigurationStorageInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var provider = configuration.GetConfigurationStorageProvider();

        if (provider.CryptoTransformer != null)
        {
            serviceCollection.AddSingleton(provider.CryptoTransformer);
        }

        return serviceCollection.AddSingleton(provider.ChangeNotifier)
            .AddSingleton<IConfigurationStorageReloader, ConfigurationStorageReloader>();
    }
}