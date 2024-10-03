using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Register;

public class RegisterExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/expenses";
    private readonly string _token;

    public RegisterExpenseTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _token = factory.User_Team_Member.GetToken();
    }
    
    [Fact]
    public async Task Success()
    {
        var request = RequestExpanseJsonBuilder.Build();
        
        var result = await DoPost(requestUri: METHOD, content: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Title_Empty(string cultureInfo)
    {
        var request = RequestExpanseJsonBuilder.Build();
        request.Title = string.Empty;
        
        var result = await DoPost(requestUri: METHOD, content: request, token: _token, culture: cultureInfo);;

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
    
}
