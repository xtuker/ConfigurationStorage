namespace Xtuker.ConfigurationStorage.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.DependencyInjection;

    public class ConfigurationDataDbContextOptions : IDbContextOptionsExtension
    {
        public string TableName { get; set; } = null!;

        public string? SchemaName { get; set; }
        
        public string KeyColumnName { get; set; } = "Key";
        public string ValueColumnName { get; set; } = "Value";
        public string EncryptedColumnName { get; set; } = "Encrypted";

        public void ApplyServices(IServiceCollection services)
        {
        }

        public void Validate(IDbContextOptions options)
        {
            if (string.IsNullOrEmpty(TableName))
            {
                throw new InvalidOperationException("table name is not set");
            }
        }

        protected virtual ConfigurationDataDbContextOptions Clone()
        {
            return new ConfigurationDataDbContextOptions(this);
        }

        public DbContextOptionsExtensionInfo Info => new ExtensionInfo(this);

        public ConfigurationDataDbContextOptions() {}

        protected ConfigurationDataDbContextOptions(ConfigurationDataDbContextOptions copyFrom)
        {
            TableName = copyFrom.TableName;
            SchemaName = copyFrom.SchemaName;
            KeyColumnName = copyFrom.KeyColumnName;
            ValueColumnName = copyFrom.ValueColumnName;
            EncryptedColumnName = copyFrom.EncryptedColumnName;
        }

        public virtual ConfigurationDataDbContextOptions WithTableName(string tableName)
        {
            var clone = Clone();
            clone.TableName = tableName;
            return clone;
        }

        public virtual ConfigurationDataDbContextOptions WithSchemaName(string schemaName)
        {
            var clone = Clone();
            clone.SchemaName = schemaName;
            return clone;
        }
        
        public virtual ConfigurationDataDbContextOptions WithTableColumns(string keyColumnName, string valueColumnName, string encryptedColumnName)
        {
            var clone = Clone();
            clone.KeyColumnName = keyColumnName;
            clone.ValueColumnName = valueColumnName;
            clone.EncryptedColumnName = encryptedColumnName;
            return clone;
        }

        private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
        {
            private string? _logFragment;
            private int? _serviceProviderHashCode;

            public ExtensionInfo(IDbContextOptionsExtension extension) : base(extension) {}

            private new ConfigurationDataDbContextOptions Extension
                => (ConfigurationDataDbContextOptions)base.Extension;

            public override bool IsDatabaseProvider => false;

            public override string LogFragment
            {
                get
                {
                    if (_logFragment == null)
                    {
                        var builder = new StringBuilder();

                        builder.Append("using db configuration table (");
                        if (Extension.SchemaName != null)
                        {
                            builder.Append($"schema: {Extension.SchemaName}, ");
                        }

                        builder.Append($"table: {Extension.TableName}) {{ {Extension.KeyColumnName}, {Extension.ValueColumnName}, {Extension.EncryptedColumnName} }}");

                        _logFragment = builder.ToString();
                    }

                    return _logFragment;
                }
            }

            public override int GetServiceProviderHashCode()
            {
                if (_serviceProviderHashCode == null)
                {
                    var hashCode = new HashCode();
                    hashCode.Add(Extension.TableName);
                    hashCode.Add(Extension.SchemaName);

                    _serviceProviderHashCode = hashCode.ToHashCode();
                }

                return _serviceProviderHashCode.Value;
            }

            public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
                => other is ExtensionInfo;

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            {
                debugInfo["ConfigurationStorage.TableName"]
                    = Extension.TableName.GetHashCode().ToString(CultureInfo.InvariantCulture);

                if (Extension.SchemaName != null)
                {
                    debugInfo["ConfigurationStorage.SchemaName"]
                        = Extension.SchemaName.GetHashCode().ToString(CultureInfo.InvariantCulture);
                }
            }
        }
    }
}