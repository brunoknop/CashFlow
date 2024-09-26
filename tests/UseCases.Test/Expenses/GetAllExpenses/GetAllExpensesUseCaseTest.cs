using CashFlow.Application.UseCases.Expenses.GetAllExpenses;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.Expenses;
using CommonTestUtilities.Services.LoggedUser;

namespace UseCases.Test.Expenses.GetAllExpenses;

public class GetAllExpensesUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var expenses = ExpenseBuilder.BuildMany(user);
        
        var useCase = CreateUseCase(user, expenses);
        
        var result = await useCase.Execute();

        var teste = "";
    }

    private GetAllExpensesUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var readRepository = new ExpensesReadOnlyRepositoryBuilder().GetAllExpensesByUser(user, expenses).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        return new GetAllExpensesUseCase(readRepository, mapper, loggedUser);
    }
}
