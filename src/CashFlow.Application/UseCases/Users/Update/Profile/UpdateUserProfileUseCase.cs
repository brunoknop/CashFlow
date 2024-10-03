using CashFlow.Communication.Requests.Users;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.UsersRepositories;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Update.Profile;

public class UpdateUserProfileUseCase(
    ILoggedUser loggedUser,
    IUserUpdateOnlyRepository updateRepository,
    IUserReadOnlyRespoitory readRepository,
    IUnityOfWork unityOfWork
) : IUpdateUserProfileUseCase
{
    public async Task Execute(RequestUpdateUserProfileJson request)
    {
        var currentUser = await loggedUser.GetLoggedUser();
        await Validate(request, currentUser.Email);
        
        var user = await updateRepository.GetById(currentUser.Id);
        
        user.Name = request.Name;
        user.Email = request.Email;
        
        updateRepository.Update(user);
        await unityOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserProfileJson request, string currentEmail)
    {
        var validator = new UpdateUserProfileValidator();
        var result = await validator.ValidateAsync(request);

        if (currentEmail.Equals(request.Email) is false)
        {
            var emailAlreadyExists = await readRepository.ExistsActiveUserWithEmail(request.Email);
        
            if (emailAlreadyExists)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_EXISTS));   
        }
        
        if (result.IsValid)
            return;
        
        var errorList = result.Errors.Select(x => x.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorList);
    }
}
