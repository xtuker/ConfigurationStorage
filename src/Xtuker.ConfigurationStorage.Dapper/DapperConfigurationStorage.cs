namespace Xtuker.ConfigurationStorage.Dapper;

using System;
using System.Collections.Generic;
using System.Data;
using global::Dapper;

internal sealed class DapperConfigurationStorage : DapperConfigurationStorage<DapperConfigurationStorage.ConfigurationData>
{
    public DapperConfigurationStorage(string connectionString,
        string sqlCommand,
        Func<string, IDbConnection> connectionFactory)
        : base(connectionString, sqlCommand, connectionFactory)
    {
    }

    internal class ConfigurationData : IConfigurationData
    {
        public string Key { get; set; } = null!;

        public string? Value { get; set; }

        public bool Encrypted { get; set; }
    }
}

internal class DapperConfigurationStorage<TConfig> : IConfigurationStorage
    where TConfig: class, IConfigurationData
{
    private readonly string _connectionString;
    private readonly string _sqlCommand;
    private readonly Func<string, IDbConnection> _connectionFactory;

    public DapperConfigurationStorage(string connectionString,
        string sqlCommand,
        Func<string, IDbConnection> connectionFactory)
    {
        _connectionString = connectionString
            ?? throw new ArgumentNullException(nameof(connectionString), "Не задана строка подключения к базе данных");
        _sqlCommand = sqlCommand
            ?? throw new ArgumentNullException(nameof(sqlCommand), "Не задана строка SQL команды получения данных");
        _connectionFactory = connectionFactory
            ?? throw new ArgumentNullException(nameof(connectionFactory), "Не задана метод создания соединения с базой данных");
    }

    public IEnumerable<IConfigurationData> GetData()
    {
        using var connection = _connectionFactory(_connectionString);
        return connection.Query<TConfig>(_sqlCommand);
    }
}