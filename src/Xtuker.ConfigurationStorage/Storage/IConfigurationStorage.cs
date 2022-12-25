namespace Xtuker.ConfigurationStorage
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using Xtuker.ConfigurationStorage.Crypto;

    /// <summary>
    /// Хранилище данных конфигурации <see cref="IConfigurationProvider"/>
    /// </summary>
    public interface IConfigurationStorage
    {
        /// <summary>
        /// Провайдер криптографических преобразований
        /// </summary>
        IConfigurationCryptoTransformer? CryptoTransformer { get; }
        
        /// <summary>
        /// Получить набор данных из хранилища
        /// </summary>
        IEnumerable<IConfigurationData> GetData();
    }


    /// <inheritdoc />
    public interface IConfigurationStorage<in TConfig> : IConfigurationStorage
        where TConfig : class, IConfigurationData
    {
        /// <summary>
        /// Сохранить значение конфигурации в хранилище
        /// </summary>
        /// <param name="config">Экземпляр конфигурации <see cref="IConfigurationData"/></param>
        void SaveData(TConfig config);
    }
}