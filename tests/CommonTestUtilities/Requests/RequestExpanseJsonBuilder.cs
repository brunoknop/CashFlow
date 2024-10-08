using Bogus;
using CashFlow.Communication.Enum;
using CashFlow.Communication.Requests.Expenses;
using TagEnum = CashFlow.Communication.Enum.Tag;

namespace CommonTestUtilities.Requests;

public class RequestExpanseJsonBuilder
{
    public static RequestExpanseJson Build()
        => new Faker<RequestExpanseJson>()
           .RuleFor(expense => expense.Title, faker => faker.Commerce.ProductName())
           .RuleFor(expense => expense.Description, faker => faker.Commerce.ProductAdjective())
           .RuleFor(expense => expense.Date, faker => faker.Date.Past())
           .RuleFor(expense => expense.PaymentType, faker => faker.PickRandom<PaymentType>())
           .RuleFor(expense => expense.Amount, faker => faker.Random.Decimal(1, 1000))
           .RuleFor(expense => expense.Tags, faker => faker.Make(1, () => faker.PickRandom<TagEnum>()));
}
