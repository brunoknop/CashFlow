using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);
        
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();
        
        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));
    }
    
    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);
        
        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_EXISTS));
    }

    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();
        var readRepository = new UserReadOnlyRespoitoryBuilder();
        var unityOfWork = UnityOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var passwordEncripter = new PasswordEncrypterBuilder().Build();
        var tokenGenerator = TokenGeneratorBuilder.Build();

        if (string.IsNullOrWhiteSpace(email) is false)
        {
            readRepository.ExistsActiveUserWithEmail(email);
        }
        
        return new RegisterUserUseCase(writeRepository, readRepository.Build(), unityOfWork, mapper, passwordEncripter, tokenGenerator);
    }
}
