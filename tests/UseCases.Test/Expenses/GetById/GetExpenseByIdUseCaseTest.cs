using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Communication.Enum;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories.Expenses;
using CommonTestUtilities.Services.LoggedUser;
using FluentAssertions;

namespace UseCases.Test.Expenses.GetById;

public class GetExpenseByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);

        var usecase = CreateUseCase(user, expense);

        var result = await usecase.Execute(expense.Id);
        
        result.Should().NotBeNull();

        result.Id.Should().Be(expense.Id);
        result.Amount.Should().Be(expense.Amount);
        result.Title.Should().Be(expense.Title);
        result.Date.Should().Be(expense.Date);
        result.Description.Should().Be(expense.Description);
        result.PaymentType.Should().Be((PaymentType)expense.PaymentType);
        result.Tags.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expense.Tags.Select(tag => tag.TagType));
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);
        
        var usecase = CreateUseCase(user);

        var act = async () => await usecase.Execute(expense.Id);

        var response = await act.Should().ThrowAsync<NotFoundException>();
        
        response.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    public GetExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var readOnlyRepository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        
        return new GetExpenseByIdUseCase(readOnlyRepository, mapper, loggedUser);
    }
}
