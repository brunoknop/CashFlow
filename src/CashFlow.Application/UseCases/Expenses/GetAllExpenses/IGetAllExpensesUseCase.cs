using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetAllExpenses;

public interface IGetAllExpensesUseCase
{
    Task<ResponseExpensesJson> Execute();
}
