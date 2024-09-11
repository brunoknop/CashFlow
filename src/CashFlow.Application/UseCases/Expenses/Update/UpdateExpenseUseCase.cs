using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IExpensesUpdateOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;

    public UpdateExpenseUseCase(
        IExpensesUpdateOnlyRepository repository,
        IUnityOfWork unityOfWork,
        IMapper mapper)
    {
        _repository = repository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
    }

    public async Task Execute(long id, RequestExpanseJson request)
    {
        Validate(request);

        var expense = await _repository.GetById(id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        _mapper.Map(request, expense);

        _repository.Update(expense);

        await _unityOfWork.Commit();
    }

    private void Validate(RequestExpanseJson request)
    {
        var validator = new ExpanseValidator();
        var result = validator.Validate(request);

        if (result.IsValid)
            return;

        var errorList = result.Errors.Select(f => f.ErrorMessage).ToList();
        throw new ErrorOnValidationException(errorList);
    }
}
