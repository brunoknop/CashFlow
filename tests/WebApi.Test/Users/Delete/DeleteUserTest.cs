using FluentAssertions;
using System.Net;

namespace WebApi.Test.Users.Delete;

public class DeleteUserTest : CashFlowClassFixture
{
    private const string METHOD = "api/users";
    private readonly string _token;

    public DeleteUserTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _token = factory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: METHOD, token: _token);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
