using CashFlow.Communication.Requests.Users;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.Update.Profile;

public class UpdateUserProfileValidator : AbstractValidator<RequestUpdateUserProfileJson>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(d => d.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(d => d.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress().When(user => string.IsNullOrWhiteSpace(user.Email) is false, ApplyConditionTo.CurrentValidator).WithMessage(ResourceErrorMessages.EMAIL_INVALID);
    }
}
