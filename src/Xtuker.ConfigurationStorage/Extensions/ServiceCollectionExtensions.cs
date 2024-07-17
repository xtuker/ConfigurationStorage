namespace Xtuker.ConfigurationStorage.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xtuker.ConfigurationStorage.Crypto;

/// <summary>
/// Extensions for <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add used services in <see cref="IConfigurationStorage"/> to <see cref="IServiceCollection"/>
    /// <remarks>
    /// <see cref="IConfigurationStorageReloader"/><br/>
    /// <see cref="IConfigurationStorageChangeNotificationService"/>
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

        return serviceCollection.AddSingleton(provider.ChangeNotificationService)
            .AddSingleton<IConfigurationStorageReloader, ConfigurationStorageReloader>();
    }
}