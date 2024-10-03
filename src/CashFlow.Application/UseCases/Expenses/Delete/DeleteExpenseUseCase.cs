using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.ExpensesRepositories;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase(
    IExpensesWriteOnlyRepository writeOnlyRepository,
    IExpensesReadOnlyRepository readOnlyRepository,
    IUnityOfWork unityOfWork,
    ILoggedUser user
) : IDeleteExpenseUseCase
{
    public async Task Execute(long id)
    {
        var loggedUser = await user.GetLoggedUser();
        
        var expense = await readOnlyRepository.GetById(loggedUser, id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        
        await writeOnlyRepository.DeleteById(id);
        await unityOfWork.Commit();
    }
}
