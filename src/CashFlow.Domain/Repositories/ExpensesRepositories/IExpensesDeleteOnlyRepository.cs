namespace CashFlow.Domain.Repositories.ExpensesRepositories;

public interface IExpensesDeleteOnlyRepository
{
    /// <summary>
    /// This function returns TRUE if the deletion was successful otherwise returns FALSE
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteById(long id);
}
