using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Expenses;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using FluentAssertions;
using DomainEnums = CashFlow.Domain.Enums;

namespace UseCases.Test.Expenses.Update;

public class UpdateExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestExpanseJsonBuilder.Build();
        var expense = ExpenseBuilder.Build(user);
        
        var useCase = CreateUseCase(user, expense);

        var act = async () => await useCase.Execute(expense.Id, request);
        await act.Should().NotThrowAsync();
        
        expense.Title.Should().Be(request.Title);
        expense.Description.Should().Be(request.Description);
        expense.Date.Should().Be(request.Date);
        expense.Amount.Should().Be(request.Amount);
        expense.PaymentType.Should().Be((DomainEnums.PaymentType)request.PaymentType);
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var user = UserBuilder.Build();
        var request = RequestExpanseJsonBuilder.Build();
        
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(id: 1000, request);

        var result = await act.Should().ThrowAsync<NotFoundException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }
    
    [Fact]
    public async Task Error_Title_Empty()
    {
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);
        var request = RequestExpanseJsonBuilder.Build();
        request.Title = string.Empty;
        
        var useCase = CreateUseCase(user, expense);

        var act = async () => await useCase.Execute(expense.Id, request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }
    
    private UpdateExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var updateRepository = new ExpensesUpdateOnlyRepositoryBuilder().GetById(user, expense).Update(expense).Build();
        var unityOfWork = UnityOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var logger = LoggedUserBuilder.Build(user);
        
        return new UpdateExpenseUseCase(updateRepository, unityOfWork, mapper, logger);
    }
}
