using Bogus;
using CashFlow.Communication.Requests.Users;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserProfileJsonBuilder
{
    public static RequestUpdateUserProfileJson Build()
        => new Faker<RequestUpdateUserProfileJson>()
           .RuleFor(user => user.Name, faker => faker.Person.FirstName)
           .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name));
}
