using AutoMapper;
using CashFlow.Communication.Requests.Users;
using CashFlow.Communication.Responses.Users;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.UsersRepositories;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Register;

public class RegisterUserUseCase(
    IUserWriteOnlyRepository userWriteOnlyRepository,
    IUserReadOnlyRespoitory userReadOnlyRepository,
    IUnityOfWork unityOfWork,
    IMapper mapper,
    IPasswordEncrypter passwordEncrypter,
    IAccessTokenGenerator tokenGenerator
) : IRegisterUserUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);
        
        var user = mapper.Map<User>(request);
        
        user.Password = passwordEncrypter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();
        
        await userWriteOnlyRepository.Add(user);
        await unityOfWork.Commit();

        return new ResponseRegisteredUserJson()
        {
            Name = user.Name,
            Token = tokenGenerator.Generate(user)
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);
        
        var emailAlreadyExists = await userReadOnlyRepository.ExistsActiveUserWithEmail(request.Email);

        if (emailAlreadyExists)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_EXISTS));
        
        if (result.IsValid)
            return;
        
        var errorList = result.Errors.Select(x => x.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorList);
    }
}
