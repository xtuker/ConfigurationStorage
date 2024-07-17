namespace UnitTests;

using Xtuker.ConfigurationStorage;
using Xtuker.ConfigurationStorage.Crypto;
using Xtuker.ConfigurationStorage.Extensions;

public partial class Tests
{
    private IConfigurationCryptoTransformer CryptoTransformer { get; set; }

    [OneTimeSetUp]
    public void Setup()
    {
        var key = System.Security.Cryptography.RandomNumberGenerator.GetBytes(32);

        var src = new ConfigurationStorageSource(null!).UseAesCryptoTransformer(key);

        CryptoTransformer = src.CryptoTransformer!;
    }

    [TestCase("test_string")]
    [TestCase("test_string546567")]
    public void SingleEncryptDecryptTest(string srcText)
    {
        var origData = new TestConfigurationData {Key = "TestKey", Value = srcText, Encrypted = true};
        var encryptedData = CryptoTransformer.Encrypt(origData);
        Assert.That(encryptedData, Is.Not.Null);
        Assert.That(encryptedData.Value, Is.Not.EqualTo(srcText));
        Print(encryptedData);
        var encValue = encryptedData.Value;

        var decryptedData = CryptoTransformer.Decrypt(encryptedData);
        Assert.That(decryptedData, Is.Not.Null);
        Assert.That(decryptedData.Value, Is.EqualTo(srcText));

        var encryptedData2 = CryptoTransformer.Encrypt(origData);
        Assert.That(encryptedData2, Is.Not.Null);
        Assert.That(encryptedData2.Value, Is.Not.EqualTo(encValue));
        Print(encryptedData2);

        var decryptedData2 = CryptoTransformer.Decrypt(encryptedData2);
        Assert.That(decryptedData2, Is.Not.Null);
        Assert.That(decryptedData2.Value, Is.EqualTo(srcText));
    }

    [Test]
    public void MultipleEncryptDecryptTest()
    {
        var configData = GenerateConfigData(100).ToList();
        Print(configData);

        var encryptedData = CryptoTransformer.Encrypt(configData).Where(x => x.Value != null).ToList();
        Assert.That(configData.Count, Is.EqualTo(encryptedData.Count));
        Print(encryptedData);

        var decryptedData = CryptoTransformer.Decrypt(encryptedData).Where(x => x.Value != null).ToList();
        Assert.That(configData.Count, Is.EqualTo(decryptedData.Count));
        Assert.That(decryptedData, Is.EquivalentTo(configData));
        Print(decryptedData);
    }

    private void Print(IConfigurationData data)
    {
#if DEBUG
        Console.WriteLine($"[{data.Key}]: {data.Value}");
#endif
    }

    private void Print(IEnumerable<IConfigurationData> data)
    {
#if DEBUG
        foreach (var d in data)
        {
            Console.WriteLine($"[{d.Key}]: {d.Value}");
        }
#endif
    }

    private IEnumerable<IConfigurationData> GenerateConfigData(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new TestConfigurationData
            {
                Key = i.ToString(),
                Value = Path.GetRandomFileName(),
                Encrypted = true
            };
        }
    }
}