namespace Xtuker.ConfigurationStorage.Crypto;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Провайдер криптографических преобразований для <see cref="IConfigurationData"/>
/// </summary>
public interface IConfigurationCryptoTransformer
{
    /// <summary>
    /// Зашифровать значение, если установлен флаг <see cref="IConfigurationData.Encrypted"/>
    /// </summary>
    /// <param name="configurationData">Настройки</param>
    /// <param name="silent">Не генерировать исключение</param>
    [return: NotNullIfNotNull(nameof(configurationData))]
    public T? Encrypt<T>(T? configurationData, bool silent = true)
        where T : class, IConfigurationData;

    /// <summary>
    /// Расшифровать значение, если установлен флаг <see cref="IConfigurationData.Encrypted"/>
    /// </summary>
    /// <param name="configurationData">Настройки</param>
    /// <param name="silent">Не генерировать исключение</param>
    [return: NotNullIfNotNull(nameof(configurationData))]
    public T? Decrypt<T>(T? configurationData, bool silent = true)
        where T : class, IConfigurationData;
        
    /// <summary>
    /// Зашифровать значения, если установлен флаг <see cref="IConfigurationData.Encrypted"/>
    /// </summary>
    /// <param name="configurationDatas">Настройки</param>
    /// <param name="silent">Не генерировать исключение</param>
    public IEnumerable<T> Encrypt<T>(IEnumerable<T> configurationDatas, bool silent = true)
        where T : class, IConfigurationData;
        
    /// <summary>
    /// Расшифровать значения, если установлен флаг <see cref="IConfigurationData.Encrypted"/>
    /// </summary>
    /// <param name="configurationDatas">Настройки</param>
    /// <param name="silent">Не генерировать исключение</param>
    public IEnumerable<T> Decrypt<T>(IEnumerable<T> configurationDatas, bool silent = true)
        where T : class, IConfigurationData;
}