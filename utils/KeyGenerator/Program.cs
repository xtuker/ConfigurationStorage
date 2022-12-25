// See https://aka.ms/new-console-template for more information

using System.Text;
using KeyGenerator;

var generator = new Generator();

var pass = args.First();
var salt = args.Skip(1).FirstOrDefault();

var passKey = generator.Generate(pass, salt);
Console.WriteLine(generator.Serialize(passKey));
Console.WriteLine(generator.Serialize(generator.Generate(pass, salt)));
Console.WriteLine(generator.Serialize(generator.Generate(pass, salt)));

var generatedKey = generator.Serialize(generator.GenerateRandom(salt));
Console.WriteLine(generatedKey);

var protector1 = new ConfigProtector("config.json", passKey);
File.WriteAllText("config.json", generatedKey, Encoding.UTF8);
protector1.Protect();

protector1.UnProtect();

var protector2 = new ConfigProtector("secret.key", passKey);
protector2.Create(generatedKey);

Console.WriteLine(protector2.Read());