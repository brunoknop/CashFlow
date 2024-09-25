using CashFlow.Communication.Requests.Users;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(d => d.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(d => d.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress().When(user => string.IsNullOrWhiteSpace(user.Email) is false, ApplyConditionTo.CurrentValidator).WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        RuleFor(d => d.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>()).WithMessage(ResourceErrorMessages.INVALID_PASSWORD);
    }
}
