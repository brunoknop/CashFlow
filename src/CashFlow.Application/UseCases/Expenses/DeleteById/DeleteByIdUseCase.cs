using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.ExpensesRepositories;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.DeleteById;

public class DeleteByIdUseCase : IDeleteByIdUseCase
{
    private readonly IExpensesDeleteOnlyRepository _writeOnlyRepository;
    private readonly IExpensesReadOnlyRepository _readOnlyRepository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteByIdUseCase(
        IExpensesDeleteOnlyRepository writeOnlyRepository,
        IExpensesReadOnlyRepository readOnlyRepository,
        IUnityOfWork unityOfWork,
        ILoggedUser loggedUser)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unityOfWork = unityOfWork;
        _loggedUser = loggedUser;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.GetLoggedUser();
        
        var expense = await _readOnlyRepository.GetById(loggedUser, id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        
        await _writeOnlyRepository.DeleteById(id);
        await _unityOfWork.Commit();
    }
}
