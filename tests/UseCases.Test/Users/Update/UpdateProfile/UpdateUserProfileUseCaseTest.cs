using CashFlow.Application.UseCases.Users.Update.Profile;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Users;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using FluentAssertions;

namespace UseCases.Test.Users.Update.UpdateProfile;

public class UpdateUserProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);
        
        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = CreateUseCase(user);
        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));
    }
    
    [Fact]
    public async Task Error_Email_Invalid()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Email = request.Name;
        
        var useCase = CreateUseCase(user);
        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_INVALID));
    }
    
    [Fact]
    public async Task Error_Email_Empty()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Email = string.Empty;
        
        var useCase = CreateUseCase(user);
        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_EMPTY));
    }
    
    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        
        var useCase = CreateUseCase(user, request.Email);
        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_EXISTS));
    }

    private static UpdateUserProfileUseCase CreateUseCase(User user, string email = "")
    {
        var logger = LoggedUserBuilder.Build(user);
        var updateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var readOnlyRepository = new UserReadOnlyRespoitoryBuilder();
        var unityOfWork = UnityOfWorkBuilder.Build();
        
        if(string.IsNullOrWhiteSpace(email) is false)
            readOnlyRepository.ExistsActiveUserWithEmail(email);
        
        return new UpdateUserProfileUseCase(logger, updateRepository, readOnlyRepository.Build(), unityOfWork);
    }
}
