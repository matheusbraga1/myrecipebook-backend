using System.Security.Cryptography;
using System.Text;

namespace MyRecipeBook.Application.Services.Cryptography;

public class PasswordEncrypter
{
    public string EncryptPassword(string password)
    {
        var additionalKey = "ABC";
        var newPassword = $"{password}{additionalKey}";

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
