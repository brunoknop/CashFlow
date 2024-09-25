using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.ExpensesRepositories;

public interface IExpensesWriteOnlyRepository
{
    Task Add(Expense expense);
}
