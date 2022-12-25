namespace KeyGenerator
{
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Json;

    public class Generator
    {
        public KeyData GenerateRandom(string? salt = null)
        {
            var saltBytes = salt != null ? Encoding.UTF8.GetBytes(salt) : null;

            var randomBytes = HKDF.DeriveKey(HashAlgorithmName.SHA512, RandomNumberGenerator.GetBytes(4096), 48, saltBytes);
            var key = Convert.ToBase64String(randomBytes);

            return new KeyData
            {
                Key = key
            };
        }
        public KeyData Generate(string password, string? salt = null)
        {
            var saltBytes = salt != null ? Encoding.UTF8.GetBytes(salt) : null;
            
            var randomBytes = HKDF.DeriveKey(HashAlgorithmName.SHA512, GenKey(4096, password), 48, saltBytes);
            var key = Convert.ToBase64String(randomBytes);

            return new KeyData
            {
                Key = key
            };
        }

        public string Serialize(KeyData keyData)
        {
            return JsonSerializer.Serialize(keyData);
        }

        private byte[] GenKey(int count, string password)
        {
            var input = Encoding.UTF8.GetBytes(password);

            return HKDF.DeriveKey(HashAlgorithmName.SHA256, input, count);
        }
    }
}