namespace Xtuker.ConfigurationStorage;

/// <summary>
/// <see cref="ConfigurationStorageProvider"/> reloader
/// </summary>
public interface IConfigurationStorageReloader
{
    /// <summary>
    /// Reload data for all registered providers
    /// </summary>
    void Reload();
}