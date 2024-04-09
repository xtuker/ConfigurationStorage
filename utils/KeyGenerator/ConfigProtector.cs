namespace KeyGenerator;

using System.Security.Cryptography;
using System.Text;

public class ConfigProtector
{
    private readonly string _fileName;
    private readonly KeyData _keyInfo;

    private string OpenFile => _fileName;
    private string ProtectedFile => _fileName + ".enc";

    public ConfigProtector(string fileName, KeyData keyInfo)
    {
        _fileName = fileName;
        _keyInfo = keyInfo;
    }
        
    public void Protect()
    {
        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(_keyInfo.Key);

        using var encryptor = aes.CreateEncryptor();
        using var frs = new FileStream(OpenFile, FileMode.Open, FileAccess.Read, FileShare.Write);
        using var fws = new FileStream(ProtectedFile, FileMode.Create, FileAccess.Write, FileShare.Read);
        using var cs = new CryptoStream(fws, encryptor, CryptoStreamMode.Write);
        {
            frs.CopyTo(cs);
        }
    }
        
    public void UnProtect()
    {
        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(_keyInfo.Key);
        using var decryptor = aes.CreateDecryptor();
        using var frs = new FileStream(ProtectedFile, FileMode.Open, FileAccess.Read, FileShare.Write);
        using var fws = new FileStream(OpenFile, FileMode.Create, FileAccess.Write, FileShare.Read);
        using var cs = new CryptoStream(frs, decryptor, CryptoStreamMode.Read);
        {
            cs.CopyTo(fws);
        }
    }

    public void Create(string configData)
    {
        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(_keyInfo.Key);

        using var encryptor = aes.CreateEncryptor();
        using var fs = new FileStream(_fileName, FileMode.Create);
        using var cs = new CryptoStream(fs, encryptor, CryptoStreamMode.Write);
        using (var sw = new StreamWriter(cs, Encoding.UTF8))
        {
            sw.Write(configData);
        }
    }

    public string Read()
    {
        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(_keyInfo.Key);

        using var decryptor = aes.CreateDecryptor();
        using var fs = new FileStream(_fileName, FileMode.Open);
        using var cs = new CryptoStream(fs, decryptor, CryptoStreamMode.Read);
        using (var sr = new StreamReader(cs, Encoding.UTF8))
        {
            return sr.ReadToEnd();
        }
    }
}