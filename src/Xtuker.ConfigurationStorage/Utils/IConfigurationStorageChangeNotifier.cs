namespace Xtuker.ConfigurationStorage;

using System;
using Microsoft.Extensions.Primitives;

/// <summary>
/// Сервис отслеживания изменений конфигурации
/// </summary>
public interface IConfigurationStorageChangeNotifier : IDisposable
{
    /// <summary>
    /// Уведомить об изменении
    /// </summary>
    void NotifyChange();

    /// <summary>
    /// Получить токен изменения
    /// </summary>
    IChangeToken CreateChangeToken();
}