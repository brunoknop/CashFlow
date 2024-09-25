using AutoMapper;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.ExpensesRepositories;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.DeleteById;

public class DeleteByIdUseCase : IDeleteByIdUseCase
{
    private readonly IExpensesDeleteOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;

    public DeleteByIdUseCase(
        IExpensesDeleteOnlyRepository repository,
        IUnityOfWork unityOfWork,
        IMapper mapper)
    {
        _repository = repository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
    }

    public async Task Execute(long id)
    {
        var deleted = await _repository.DeleteById(id);

        if (deleted is false)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await _unityOfWork.Commit();
    }
}
