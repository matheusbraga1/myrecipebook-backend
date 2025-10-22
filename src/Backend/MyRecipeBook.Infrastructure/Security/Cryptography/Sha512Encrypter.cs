using MyRecipeBook.Domain.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.Cryptography;

public class Sha512Encrypter : IPasswordEncrypter
{
    private readonly string _additionalKey;

    public Sha512Encrypter(string additionalKey) => _additionalKey = additionalKey;

    public string Encrypt(string password)
    {
        var newPassword = $"{password}{_additionalKey}";

        var bytes = Encoding.UTF8.GetBytes(newPassword);
        var hashBytes = SHA512.HashData(bytes);

        return ConvertToStringBytes(hashBytes);
    }

    private static string ConvertToStringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (var b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }
}
