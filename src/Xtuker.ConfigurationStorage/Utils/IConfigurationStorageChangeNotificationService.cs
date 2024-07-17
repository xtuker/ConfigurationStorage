namespace Xtuker.ConfigurationStorage;

using System;
using Microsoft.Extensions.Primitives;

/// <summary>
/// <see cref="ConfigurationStorageProvider"/> change notification service
/// </summary>
public interface IConfigurationStorageChangeNotificationService : IDisposable
{
    /// <summary>
    /// Tells the provider to reload the data
    /// </summary>
    void NotifyChange();

    /// <summary>
    /// Create new change token
    /// </summary>
    /// <seealso cref="IChangeToken"/>
    IChangeToken CreateChangeToken();
}