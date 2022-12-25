namespace Xtuker.ConfigurationStorage
{
    using Microsoft.Extensions.Configuration;
    using Xtuker.ConfigurationStorage.Extensions;

    /// <inheritdoc />
    internal class ConfigurationStorageReloader : IConfigurationStorageReloader
    {
        private readonly ConfigurationStorageProvider _storageProvider;

        /// <summary>
        /// .ctor
        /// </summary>
        public ConfigurationStorageReloader(IConfiguration configuration)
        {
            _storageProvider = configuration.GetConfigurationStorageProvider();
        }

        /// <inheritdoc />
        public void Reload()
        {
            _storageProvider.Load();
        }
    }
}