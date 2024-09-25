using AutoMapper;
using CashFlow.Communication.Requests.Expenses;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.ExpensesRepositories;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggerUser;

    public RegisterExpenseUseCase(
        IExpensesWriteOnlyRepository repository,
        IUnityOfWork unityOfWork,
        IMapper mapper,
        ILoggedUser loggerUser)
    {
        _repository = repository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
        _loggerUser = loggerUser;
    }

    public async Task<ResponseRegisteredExpenseJson> Execute(RequestExpanseJson request)
    {
        Validate(request);

        var loggedUser = await _loggerUser.GetLoggedUser();
        
        var expense = _mapper.Map<Expense>(request);
        expense.UserId = loggedUser.Id;

        await _repository.Add(expense);
        await _unityOfWork.Commit();

        return _mapper.Map<ResponseRegisteredExpenseJson>(expense);
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
