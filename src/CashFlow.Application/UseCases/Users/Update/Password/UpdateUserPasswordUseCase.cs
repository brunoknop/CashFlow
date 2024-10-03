using CashFlow.Communication.Requests.Users;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.UsersRepositories;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Update.Password;

public class UpdateUserPasswordUseCase(
    ILoggedUser loggedUser,
    IPasswordEncrypter passwordEncrypter,
    IUserUpdateOnlyRepository updateRepository,
    IUnityOfWork unityOfWork
) : IUpdateUserPasswordUseCase
{
    public async Task Execute(RequestUpdateUserPasswordJson request)
    {
        var currentUser = await loggedUser.GetLoggedUser();
        Validate(request, currentUser);
        
        var user = await updateRepository.GetById(currentUser.Id);
        
        user.Password = passwordEncrypter.Encrypt(request.NewPassword);
        
        updateRepository.Update(user);
        await unityOfWork.Commit();
    }

    private void Validate(RequestUpdateUserPasswordJson request, User loggedUser)
    {
        var validator = new UpdateUserPasswordValidation();
        var result = validator.Validate(request);

        var currentPasswordMatch = passwordEncrypter.Validate(request.CurrentPassword, loggedUser.Password);
        if(currentPasswordMatch is false)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        
        if (result.IsValid)
            return;
        
        var errorList = result.Errors.Select(x => x.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorList);
    }
}
