namespace Xtuker.ConfigurationStorage
{
    /// <summary>
    /// Сервиз перезагрузки конфигурации из хранилища <see cref="IConfigurationStorage"/>
    /// </summary>
    public interface IConfigurationStorageReloader
    {
        /// <summary>
        /// Перезагрузить конфигурацию
        /// </summary>
        void Reload();
    }
}