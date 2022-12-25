namespace Xtuker.ConfigurationStorage
{
    /// <summary>
    /// Конфигурация без шифрования
    /// </summary>
    public class ConfigurationData : IConfigurationData
    {
        /// <inheritdoc />
        public string Key { get; set; } = null!;

        /// <inheritdoc />
        public string? Value { get; set; }

        /// <inheritdoc />
        bool IConfigurationData.Encrypted => false;
    }
}