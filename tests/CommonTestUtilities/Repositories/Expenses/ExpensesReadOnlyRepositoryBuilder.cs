using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.ExpensesRepositories;
using Moq;

namespace CommonTestUtilities.Repositories.Expenses;

public class ExpensesReadOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesReadOnlyRepository> _repository;

    public ExpensesReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IExpensesReadOnlyRepository>();
    }

    public ExpensesReadOnlyRepositoryBuilder GetAllExpensesByUser(User user, List<Expense> expenses)
    {
        _repository.Setup(readRepository => readRepository.GetAll(user)).ReturnsAsync(expenses);
        return this;
    }
    
    public IExpensesReadOnlyRepository Build() => _repository.Object;
}
