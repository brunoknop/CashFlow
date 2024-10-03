using Bogus;
using CashFlow.Communication.Requests.Users;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserPasswordJsonBuilder
{
    public static RequestUpdateUserPasswordJson Build()
        => new Faker<RequestUpdateUserPasswordJson>()
           .RuleFor(request => request.CurrentPassword, faker => faker.Internet.Password(prefix: "!Aa1"))
           .RuleFor(request => request.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
}
