using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Expenses;
using CommonTestUtilities.Services.LoggedUser;
using FluentAssertions;

namespace UseCases.Test.Expenses.Delete;

public class DeleteExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);

        var useCase = CreateUseCase(user, expense);

        var act = async () => await useCase.Execute(expense.Id);
        await act.Should().NotThrowAsync();
        
        
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var user = UserBuilder.Build();
      
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }


    private DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var writeRepository = ExpensesWriteOnlyRepositoryBuilder.Build(expense);
        var readRepository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var unityOfWork = UnityOfWorkBuilder.Build();
        var logger = LoggedUserBuilder.Build(user);
        return new DeleteExpenseUseCase(writeRepository,readRepository, unityOfWork, logger);
    }
}
