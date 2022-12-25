namespace Xtuker.ConfigurationStorage.Extensions
{
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
        public static IServiceCollection AddConfigurationStorageInfrastructure(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<IConfigurationStorageReloader, ConfigurationStorageReloader>()
                .AddSingleton<IConfigurationStorage>(x => x.GetRequiredService<IConfiguration>().GetConfigurationStorageProvider().Storage
                    ?? throw new NotSupportedException($"{nameof(IConfigurationStorage)} does not used"))
                .AddSingleton<IConfigurationCryptoTransformer>(x => x.GetRequiredService<IConfigurationStorage>().CryptoTransformer
                    ?? throw new NotSupportedException($"{nameof(IConfigurationCryptoTransformer)} does not used"));
        }
    }
}