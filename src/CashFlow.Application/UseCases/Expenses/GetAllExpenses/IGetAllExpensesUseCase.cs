using CashFlow.Communication.Responses.Expenses;

namespace CashFlow.Application.UseCases.Expenses.GetAllExpenses;

public interface IGetAllExpensesUseCase
{
    Task<ResponseExpensesJson> Execute();
}
