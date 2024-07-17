namespace UnitTests;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xtuker.ConfigurationStorage;
using Xtuker.ConfigurationStorage.Crypto;
using Xtuker.ConfigurationStorage.Extensions;

public class InMemoryStorageTests
{
    private InMemoryStorage Storage { get; set; }
    private IConfiguration Configuration { get; set; }
    private IConfigurationStorageReloader Reloader { get; set; }

    [OneTimeSetUp]
    public void Setup()
    {
        var storage = new InMemoryStorage(new[]
        {
            new TestConfigurationData {Key = "One", Value = "Plane text", Encrypted = false},
            new TestConfigurationData {Key = "Two", Value = "Two plane text", Encrypted = false}
        });

        var key = System.Security.Cryptography.RandomNumberGenerator.GetBytes(32);

        Storage = storage;
        var configurationRoot = new ConfigurationBuilder()
            .AddStorage(Storage,
                c =>
                {
                    c.UseAesCryptoTransformer(key);
                })
            .Build();

        var sv = new ServiceCollection();
        sv.AddSingleton<IConfiguration>(configurationRoot);
        sv.AddConfigurationStorageInfrastructure(configurationRoot);
        var sp = sv.BuildServiceProvider();

        Reloader = sp.GetRequiredService<IConfigurationStorageReloader>();
        Configuration = sp.GetRequiredService<IConfiguration>();
        var cryptoService = sp.GetRequiredService<IConfigurationCryptoTransformer>();

        storage.SaveData(cryptoService.Encrypt(new TestConfigurationData
        {
            Key = "Secret",
            Value = "Secret value",
            Encrypted = true
        }, true));

        Reload();
    }

    [TestCase("One", "Plane text")]
    [TestCase("Two", "Two plane text")]
    [TestCase("Secret", "Secret value")]
    public void ExistsValueTest(string key, string value)
    {
        var config = Configuration[key];
        Assert.That(config, Is.EqualTo(value));
    }

    [TestCase("Secret")]
    public void NotEqualsEncryptedValueTest(string key)
    {
        var config = Configuration[key];
        Assert.That(config, Is.Not.EqualTo(Storage.Get(key)));
    }

    private void Reload()
    {
        Reloader.Reload();
    }
}