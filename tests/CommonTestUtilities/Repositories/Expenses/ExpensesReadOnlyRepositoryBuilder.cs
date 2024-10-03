using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.ExpensesRepositories;
using Moq;

namespace CommonTestUtilities.Repositories.Expenses;

public class ExpensesReadOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesReadOnlyRepository> _mock;

    public ExpensesReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IExpensesReadOnlyRepository>();
    }

    public ExpensesReadOnlyRepositoryBuilder GetAll(User user, List<Expense> expenses)
    {
        _mock.Setup(readRepository => readRepository.GetAll(user)).ReturnsAsync(expenses);
        return this;
    }
    
    public ExpensesReadOnlyRepositoryBuilder GetById(User user, Expense? expense = null)
    {
        if (expense is not null)
            _mock.Setup(readRepository => readRepository.GetById(user, expense.Id)).ReturnsAsync(expense);
        return this;
    }
    
    public ExpensesReadOnlyRepositoryBuilder FilterByMonth(User user, List<Expense> expenses)
    {
        _mock.Setup(readRepository => readRepository.FilterByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(expenses);
        return this;
    }
    
    
    
    public IExpensesReadOnlyRepository Build() => _mock.Object;
}
