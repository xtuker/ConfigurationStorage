namespace Xtuker.ConfigurationStorage;

using System.Collections.Generic;
using Xtuker.ConfigurationStorage.Crypto;

/// <summary>
/// Базовая реализация <see cref="IConfigurationStorage"/>
/// </summary>
public abstract class BaseConfigurationReadOnlyStorage : IConfigurationStorage
{
    /// <inheritdoc />
    public IConfigurationCryptoTransformer? CryptoTransformer { get; }

    /// <summary>
    /// .ctor
    /// </summary>
    protected BaseConfigurationReadOnlyStorage(IConfigurationCryptoTransformer? cryptoTransformer)
    {
        CryptoTransformer = cryptoTransformer;
    }

    /// <summary>
    /// Получить набор данных из хранилища
    /// </summary>
    protected abstract IEnumerable<IConfigurationData> GetDataInternal();

    /// <inheritdoc />
    public virtual IEnumerable<IConfigurationData> GetData()
    {
        return CryptoTransformer == null ? GetDataInternal() : CryptoTransformer.Decrypt(GetDataInternal());
    }
}

/// <summary>
/// Базовая реализация <see cref="IConfigurationStorage"/>
/// </summary>
public abstract class BaseConfigurationStorage<TConfig> : BaseConfigurationReadOnlyStorage, IConfigurationStorage<TConfig>
    where TConfig : IConfigurationData
{
    /// <summary>
    /// .ctor
    /// </summary>
    protected BaseConfigurationStorage(IConfigurationCryptoTransformer? cryptoTransformer)
        : base(cryptoTransformer)
    {
    }
    /// <summary>
    /// Сохранить данные в хранилище
    /// </summary>
    protected abstract void SetDataInternal(TConfig config);

    /// <inheritdoc />
    public virtual void SaveData(TConfig config)
    {
        SetDataInternal(CryptoTransformer == null ? config : CryptoTransformer.Encrypt(config));
    }
}