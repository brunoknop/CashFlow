using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.ExpensesRepositories;
using Moq;

namespace CommonTestUtilities.Repositories.Expenses;

public class ExpensesUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesUpdateOnlyRepository> _mock;

    public ExpensesUpdateOnlyRepositoryBuilder()
    {
        _mock = new Mock<IExpensesUpdateOnlyRepository>();
    }

    public ExpensesUpdateOnlyRepositoryBuilder GetById(User user, Expense? expense = null)
    {
        if (expense is not null)
            _mock.Setup(updateRepository => updateRepository.GetById(user, expense.Id)).ReturnsAsync(expense);
        return this;
    }

    public ExpensesUpdateOnlyRepositoryBuilder Update(Expense? expense)
    {
        if (expense is not null)
            _mock.Setup(updateRepository => updateRepository.Update(expense));
        return this;
    }
    
    public IExpensesUpdateOnlyRepository Build() => _mock.Object;
}
