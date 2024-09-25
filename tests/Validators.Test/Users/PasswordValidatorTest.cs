using CashFlow.Application.UseCases.Users;
using CashFlow.Communication.Requests.Users;
using FluentAssertions;
using FluentValidation;

namespace Validators.Test.Users;

public class PasswordValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("aaaaaaaaa")]
    [InlineData("AAAAAAAA")]
    [InlineData("AaAAAAAA")]
    [InlineData("AaAAAAA1")]
    public void Error_Invalid_Password(string password)
    {
        var validator = new PasswordValidator<RequestRegisterUserJson>();
        
        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);
        
        result.Should().BeFalse();
    }
}
