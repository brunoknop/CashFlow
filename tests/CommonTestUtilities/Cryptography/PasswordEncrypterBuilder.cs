using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncrypter> _mock;

    public PasswordEncrypterBuilder()
    {
        _mock = new Mock<IPasswordEncrypter>();
        _mock.Setup(passwordEncripter => passwordEncripter.Encrypt(It.IsAny<string>())).Returns("PASSWORD_ENCRIPTED");
    }

    public PasswordEncrypterBuilder Validate(string? password)
    {
        if (string.IsNullOrWhiteSpace(password) is false)
            _mock.Setup(passwordEncrypter => passwordEncrypter.Validate(password, It.IsAny<string>())).Returns(true);
        
        return this;
    }
    public IPasswordEncrypter Build() => _mock.Object;
}
