namespace CashFlow.Domain.Security.Cryptography;

public interface IPasswordEncrypter
{
    string Encrypt(string password);
    
    bool Validate(string password, string passdordHash);
}
