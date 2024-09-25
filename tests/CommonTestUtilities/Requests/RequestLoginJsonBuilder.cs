using Bogus;
using CashFlow.Communication.Requests.Users;

namespace CommonTestUtilities.Requests;

public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
        => new Faker<RequestLoginJson>()
           .RuleFor(user => user.Email, faker => faker.Internet.Email())
           .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
}
