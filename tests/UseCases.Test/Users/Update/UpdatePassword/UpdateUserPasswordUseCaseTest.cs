using CashFlow.Application.UseCases.Users.Update.Password;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using FluentAssertions;

namespace UseCases.Test.Users.Update.UpdatePassword;

public class UpdateUserPasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserPasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.CurrentPassword);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Invalid_Password()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserPasswordJsonBuilder.Build();
        request.NewPassword = "123456";

        var useCase = CreateUseCase(user, request.CurrentPassword);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.INVALID_PASSWORD));
    }

    [Fact]
    public async Task Error_Current_Passwords_Are_Different()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserPasswordJsonBuilder.Build();
        request.CurrentPassword = request.NewPassword;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
    }

    private UpdateUserPasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var logger = LoggedUserBuilder.Build(user);
        var passwordEncrypter = new PasswordEncrypterBuilder().Validate(password).Build();
        var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var unityOfWork = UnityOfWorkBuilder.Build();
        
        return new UpdateUserPasswordUseCase(logger, passwordEncrypter, updateRepository, unityOfWork);
    }
}
