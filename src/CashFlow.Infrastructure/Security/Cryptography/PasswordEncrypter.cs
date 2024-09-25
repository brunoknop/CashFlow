using CashFlow.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infrastructure.Security.Cryptography;

public class PasswordEncrypter : IPasswordEncrypter
{
    public string Encrypt(string password)
        => BC.HashPassword(password);
    
    public bool Validate(string password, string passdordHash) => BC.Verify(password, passdordHash);
}
