using CashFlow.Domain.Repositories.ExpensesRepositories;
using Moq;

namespace CommonTestUtilities.Repositories.Expenses;

public static class ExpensesWriteOnlyRepositoryBuilder
{
    public static IExpensesWriteOnlyRepository Build()
    {
        var mock = new Mock<IExpensesWriteOnlyRepository>();
        return mock.Object;
    }
}
