using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Users.GetProfile;

public class GetProfileTest : CashFlowClassFixture
{
    private const string METHOD = "api/users";
    private readonly string _token;

    public GetProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _token = factory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: METHOD, token: _token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        
        response.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("email").GetString().Should().NotBeNullOrWhiteSpace();
    }
}
