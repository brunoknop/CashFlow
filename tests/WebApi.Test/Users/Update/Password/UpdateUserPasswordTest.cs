using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Update.Password;

public class UpdateUserPasswordTest : CashFlowClassFixture
{
    private const string METHOD = "api/users";
    private readonly string _token;
    private readonly string _password;

    public UpdateUserPasswordTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _token = factory.User_Team_Member.GetToken();
        _password = factory.User_Team_Member.GetPassword();
    }
    
    
    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserPasswordJsonBuilder.Build();
        request.CurrentPassword = _password;
        
        var result = await DoPut(requestUri: $"{METHOD}/change-password", token: _token, content: request);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Current_Password_Different(string cultureInfo)
    {
        var request = RequestUpdateUserPasswordJsonBuilder.Build();
        
        var result = await DoPut(requestUri: $"{METHOD}/change-password", token: _token, content: request, culture: cultureInfo);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

}
