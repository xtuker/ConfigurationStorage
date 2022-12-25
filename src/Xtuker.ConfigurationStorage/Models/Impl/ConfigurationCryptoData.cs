namespace Xtuker.ConfigurationStorage
{
    /// <summary>
    /// Конфигурация с настраиваемым шифрованием
    /// </summary>
    public class ConfigurationCryptoData : ConfigurationData, IConfigurationData
    {
        /// <inheritdoc />
        public bool Encrypted { get; set; }
    }
}