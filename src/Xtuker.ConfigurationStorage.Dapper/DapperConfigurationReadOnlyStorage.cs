namespace Xtuker.ConfigurationStorage.Dapper;

using System;
using System.Collections.Generic;
using System.Data;
using global::Dapper;
using Xtuker.ConfigurationStorage.Crypto;

internal sealed class DapperConfigurationReadOnlyStorage : DapperConfigurationReadOnlyStorage<DapperConfigurationReadOnlyStorage.ConfigurationData>
{
    public DapperConfigurationReadOnlyStorage(string connectionString,
        string sqlCommand,
        Func<string, IDbConnection> connectionFactory,
        IConfigurationCryptoTransformer? cryptoTransformer)
        : base(connectionString, sqlCommand, connectionFactory, cryptoTransformer)
    {
    }

    internal class ConfigurationData : IConfigurationData
    {
        public string Key { get; set; } = null!;

        public string? Value { get; set; }

        public bool Encrypted { get; set; }
    }
}

internal class DapperConfigurationReadOnlyStorage<TConfig> : BaseConfigurationReadOnlyStorage
    where TConfig: class, IConfigurationData
{
    private readonly string _connectionString;
    private readonly string _sqlCommand;
    private readonly Func<string, IDbConnection> _connectionFactory;

    public DapperConfigurationReadOnlyStorage(string connectionString,
        string sqlCommand,
        Func<string, IDbConnection> connectionFactory,
        IConfigurationCryptoTransformer? cryptoTransformer)
        : base(cryptoTransformer)
    {
        _connectionString = connectionString
            ?? throw new ArgumentNullException(nameof(connectionString), "Не задана строка подключения к базе данных");
        _sqlCommand = sqlCommand
            ?? throw new ArgumentNullException(nameof(sqlCommand), "Не задана строка SQL команды получения данных");
        _connectionFactory = connectionFactory
            ?? throw new ArgumentNullException(nameof(connectionFactory), "Не задана метод создания соединения с базой данных");
    }

    protected override IEnumerable<IConfigurationData> GetDataInternal()
    {
        using var connection = _connectionFactory(_connectionString);
        return connection.Query<TConfig>(_sqlCommand);
    }
}