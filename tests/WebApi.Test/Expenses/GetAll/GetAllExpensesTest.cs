using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetAll;

public class GetAllExpensesTest : CashFlowClassFixture
{
    private const string METHOD = "api/expenses";
    private readonly string _token;

    public GetAllExpensesTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _token = factory.User_Team_Member.GetToken();
    }

    [Fact]
    private async Task Success()
    {
        var result = await DoGet(requestUri: METHOD, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        
        var expenses = response.RootElement.GetProperty("expenses").EnumerateArray().ToList();
        expenses.Should().NotBeNull().And.AllSatisfy(expense =>
        {
            expense.GetProperty("id").GetInt64().Should().BeGreaterThan(0);
            expense.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);
            expense.GetProperty("title").GetString().Should().NotBeNullOrEmpty();
        });
    }
}
