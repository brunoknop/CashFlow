namespace CashFlow.Domain.Repositories.ExpensesRepositories;

public interface IExpensesDeleteOnlyRepository
{
    Task DeleteById(long id);
}
