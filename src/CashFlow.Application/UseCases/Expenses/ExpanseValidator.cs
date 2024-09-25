using CashFlow.Communication.Requests.Expenses;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses;

public class ExpanseValidator : AbstractValidator<RequestExpanseJson>
{
    public ExpanseValidator()
    {
        RuleFor(register => register.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_REQUIRED);
        RuleFor(register => register.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
        RuleFor(register => register.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.EXPENSES_CANNOT_BE_FOR_THE_FUTURE);
        RuleFor(register => register.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_IS_INVALID);
    }
}
