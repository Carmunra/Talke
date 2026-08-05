using BCrypt.Net;

namespace Talke.Infrastructure.Security;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);

}

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        //Gera um hash da senha usando o algoritmo BCrypt
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}