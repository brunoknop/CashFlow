using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Expenses;
using CommonTestUtilities.Services.LoggedUser;
using FluentAssertions;

namespace UseCases.Test.Expenses.Reports.Excel;

public class GenerateExpenseReportExcelUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var expenses = ExpenseBuilder.Colletion(user);
        var useCase = CreateUseCase(user, expenses);

        var result = await useCase.Execute(new DateOnly());
        
        result.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task Success_With_Empty()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user, []);

        var result = await useCase.Execute(new DateOnly());
        
        result.Should().BeNullOrEmpty();
    }
    
    private static GenerateExpenseReportExcelUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var readRepository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(user, expenses).Build();
        var logger = LoggedUserBuilder.Build(user);
        return new GenerateExpenseReportExcelUseCase(readRepository, logger);
    }
}
