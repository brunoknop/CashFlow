using FluentAssertions;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Expenses.Reports;

public class ReportExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/report";
    private readonly DateTime _date;
    private readonly string _adminToken;
    private readonly string _teamMemberToken;

    public ReportExpenseTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _date = factory.Admin_Expense.GetDate();
        _adminToken = factory.User_Admin.GetToken();
        _teamMemberToken = factory.User_Team_Member.GetToken();
    }
    
    
    [Fact]
    public async Task Pdf_Success()
    {
        var result = await DoGet(requestUri: $"api/report/pdf?date={_date:yyyy-MM}", token: _adminToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        
        result.Content.Headers.ContentType!.MediaType.Should().NotBeNull();
        result.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }
    
    [Fact]
    public async Task Excel_Success()
    {
        var result = await DoGet(requestUri: $"api/report/excel?date={_date:yyyy-MM}", token: _adminToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        
        result.Content.Headers.ContentType!.MediaType.Should().NotBeNull();
        result.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await DoGet(requestUri: $"api/report/pdf?date={_date:Y}", token: _teamMemberToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGet(requestUri: $"api/report/excel?date={_date:Y}", token: _teamMemberToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
