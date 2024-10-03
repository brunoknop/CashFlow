using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.Expenses;
using CommonTestUtilities.Services.LoggedUser;
using FluentAssertions;

namespace UseCases.Test.Expenses.Reports.Pdf;

public class GenerateExpenseReportPdfUseCaseTest
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
    
    private GenerateExpenseReportPdfUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var readRepository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(user, expenses).Build();
        var logger = LoggedUserBuilder.Build(user);
        return new GenerateExpenseReportPdfUseCase(readRepository, logger);
    }
}
