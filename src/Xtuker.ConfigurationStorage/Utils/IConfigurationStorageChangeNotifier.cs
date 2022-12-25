namespace Xtuker.ConfigurationStorage
{
    using System;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Сервис отслеживания изменений конфигурации
    /// </summary>
    public interface IConfigurationStorageChangeNotifier : IDisposable
    {
        /// <summary>
        /// Получить токен изменения
        /// </summary>
        IChangeToken NotifyChange();
    }
}