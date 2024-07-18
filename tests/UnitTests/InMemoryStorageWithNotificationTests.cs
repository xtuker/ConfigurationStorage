namespace UnitTests;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xtuker.ConfigurationStorage.Crypto;
using Xtuker.ConfigurationStorage.Extensions;

public class InMemoryStorageWithNotificationTests
{
    private InMemoryStorage Storage { get; set; }
    private IConfiguration Configuration { get; set; }
    private IConfigurationCryptoTransformer CryptoTransformer { get; set; }

    [OneTimeSetUp]
    public void Setup()
    {
        var storage = new InMemoryStorageWithNotification(new[]
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
                    c.UseAesCryptoTransformer(key)
                        .UseChangeNotificationService(storage.ChangeNotificationService);
                })
            .Build();

        var sv = new ServiceCollection();
        sv.AddConfigurationStorageInfrastructure(configurationRoot);
        var sp = sv.BuildServiceProvider();

        Configuration = configurationRoot;
        CryptoTransformer = sp.GetRequiredService<IConfigurationCryptoTransformer>();
    }

    [Test]
    public void ReloadTest()
    {
        var key = "Secret";
        var value = "Very secret value";

        var notFoundConfig = Configuration[key];
        Assert.That(notFoundConfig, Is.Null.Or.Empty);

        Storage.SaveData(CryptoTransformer.Encrypt(new TestConfigurationData
        {
            Key = key,
            Value = value,
            Encrypted = true
        }, true));

        var config = Configuration[key];
        Assert.Multiple(() =>
        {
            Assert.That(config, Is.EqualTo(value));
            Assert.That(config, Is.Not.EqualTo(Storage.Get(key)));
        });
    }
}