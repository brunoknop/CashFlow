using AutoMapper;
using CashFlow.Communication.Requests.Expenses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.ExpensesRepositories;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IExpensesUpdateOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public UpdateExpenseUseCase(
        IExpensesUpdateOnlyRepository repository,
        IUnityOfWork unityOfWork,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task Execute(long id, RequestExpanseJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.GetLoggedUser();
        
        var expense = await _repository.GetById(loggedUser, id);

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
