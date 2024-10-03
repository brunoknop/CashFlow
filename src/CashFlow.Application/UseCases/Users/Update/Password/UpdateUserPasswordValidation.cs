using CashFlow.Communication.Requests.Users;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.Update.Password;

public class UpdateUserPasswordValidation : AbstractValidator<RequestUpdateUserPasswordJson>
{
    public UpdateUserPasswordValidation()
    {
        RuleFor(request => request.NewPassword).SetValidator(new PasswordValidator<RequestUpdateUserPasswordJson>()).WithMessage(ResourceErrorMessages.INVALID_PASSWORD);
    }
}
