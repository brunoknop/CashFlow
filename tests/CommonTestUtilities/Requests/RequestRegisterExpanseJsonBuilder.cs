using Bogus;
using CashFlow.Communication.Enum;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterExpanseJsonBuilder
{
    public static RequestExpanseJson Build()
    {
        var faker = new Faker();

        return new Faker<RequestExpanseJson>()
               .RuleFor(r => r.Title, f => f.Commerce.ProductName())
               .RuleFor(r => r.Description, f => f.Commerce.ProductAdjective())
               .RuleFor(r => r.Date, f => f.Date.Past())
               .RuleFor(r => r.PaymentType, f => f.PickRandom<PaymentType>())
               .RuleFor(r => r.Amount, f => f.Random.Decimal(1, 1000));
    }
}
