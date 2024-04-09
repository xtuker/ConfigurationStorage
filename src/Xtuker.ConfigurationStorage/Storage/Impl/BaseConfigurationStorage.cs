namespace Xtuker.ConfigurationStorage;

using System.Collections.Generic;
using Xtuker.ConfigurationStorage.Crypto;

    
/// <summary>
/// Базовая реализация <see cref="IConfigurationStorage"/>
/// </summary>
public abstract class BaseConfigurationStorage<TConfig> : IConfigurationStorage<TConfig>
    where TConfig : class, IConfigurationData
{
    /// <inheritdoc />
    public IConfigurationCryptoTransformer? CryptoTransformer { get; }
        
    /// <summary>
    /// .ctor
    /// </summary>
    protected BaseConfigurationStorage(IConfigurationCryptoTransformer? cryptoTransformer = null)
    {
        CryptoTransformer = cryptoTransformer;
    }
        
    /// <summary>
    /// Сохранить данные в хранилище
    /// </summary>
    protected abstract void SetDataInternal(TConfig config);
        
    /// <summary>
    /// Получить набор данных из хранилища
    /// </summary>
    protected abstract IEnumerable<TConfig> GetDataInternal();

    /// <inheritdoc />
    public virtual void SaveData(TConfig config)
    {
        SetDataInternal(CryptoTransformer == null ? config : CryptoTransformer.Encrypt(config));
    }

    /// <inheritdoc />
    public virtual IEnumerable<IConfigurationData> GetData()
    {
        return CryptoTransformer == null ? GetDataInternal() : CryptoTransformer.Decrypt(GetDataInternal());
    }
}