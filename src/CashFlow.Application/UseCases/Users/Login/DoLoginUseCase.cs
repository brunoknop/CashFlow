using CashFlow.Communication.Requests.Users;
using CashFlow.Communication.Responses.Users;
using CashFlow.Domain.Repositories.UsersRepositories;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Users.Login;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRespoitory _UserReadOnlyRespoitory;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public DoLoginUseCase(IUserReadOnlyRespoitory userReadOnlyRespoitory, IPasswordEncrypter passwordEncrypter, IAccessTokenGenerator accessTokenGenerator)
    {
        _UserReadOnlyRespoitory = userReadOnlyRespoitory;
        _passwordEncrypter = passwordEncrypter;
        _accessTokenGenerator = accessTokenGenerator;
    }
    
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var user = await _UserReadOnlyRespoitory.GetUserByEmail(request.Email);

        if (user is null) throw new InvalidLoginException();
        
        var passwordMatch = _passwordEncrypter.Validate(request.Password, user.Password);
        
        if (passwordMatch is false) throw new InvalidLoginException();
        
        return new ResponseRegisteredUserJson()
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user),
        };
    }
}
