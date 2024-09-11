namespace CashFlow.Application.UseCases.Expenses.DeleteById;

public interface IDeleteByIdUseCase
{
    Task Execute(long id);
}
