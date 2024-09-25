using CashFlow.Communication.Requests.Users;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string METHOD = "api/login";
    private readonly HttpClient _httpClient;
    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public DoLoginTest(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
        _email = factory.GetEmail();
        _name = factory.GetName();
        _password = factory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson()
        {
            Email = _email,
            Password = _password
        };
        
        var response = await _httpClient.PostAsJsonAsync(METHOD, request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        responseData.RootElement.GetProperty("name").GetString().Should().Be(_name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_On_Login(string cultureInfo)
    {
        var request = RequestLoginJsonBuilder.Build();
        
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));
        
        var response = await _httpClient.PostAsJsonAsync(METHOD, request);
        
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
