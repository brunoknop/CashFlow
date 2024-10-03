using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Update.Profile;

public class UpdateUserProfileTest : CashFlowClassFixture
{
    private const string METHOD = "api/users";
    private readonly string _token;
    private readonly string _password;

    public UpdateUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _token = factory.User_Team_Member.GetToken();
        _password = factory.User_Team_Member.GetPassword();
    }
    
    
    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        
        var result = await DoPut(requestUri: METHOD, token: _token, content: request);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Name_Empty(string cultureInfo)
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = string.Empty;
        
        var result = await DoPut(requestUri: METHOD, token: _token, content: request, culture: cultureInfo);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

}
