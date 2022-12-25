namespace Xtuker.ConfigurationStorage
{
    /// <summary>
    /// Данные конфигурации
    /// </summary>
    public interface IConfigurationData
    {
        /// <summary>
        /// Ключ
        /// <example>Foo:Bar:Baz</example>
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Значение
        /// </summary>
        string? Value { get; set; }
        
        /// <summary>
        /// Признак что значение должно быть зашифровано перед сохранением
        /// </summary>
        bool Encrypted { get; }
    }
}