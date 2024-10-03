using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.ExpensesRepositories;
using Moq;

namespace CommonTestUtilities.Repositories.Expenses;

public static class ExpensesWriteOnlyRepositoryBuilder
{
    public static IExpensesWriteOnlyRepository Build(Expense? expense = null)
    {
        var mock = new Mock<IExpensesWriteOnlyRepository>();
        if (expense != null)
            mock.Setup(deleteRepository => deleteRepository.DeleteById(expense.Id));
        return mock.Object;
    }
}
