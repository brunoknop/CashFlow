using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Register;

public class RegisterUserTest : CashFlowClassFixture
{
    private const string METHOD = "api/users";

    public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await DoPost(requestUri: METHOD, content: request);
        
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        
        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Name_Empty(string cultureInfo)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var result = await DoPost(requestUri: METHOD, content: request, culture: cultureInfo);
        
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
