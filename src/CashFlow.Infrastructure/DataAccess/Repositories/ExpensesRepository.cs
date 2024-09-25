using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.ExpensesRepositories;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository :
    IExpensesReadOnlyRepository,
    IExpensesWriteOnlyRepository,
    IExpensesDeleteOnlyRepository,
    IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;

    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task<List<Expense>> GetAll()
    {
        return await _dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    public async Task<List<Expense>> FilterByMonth(DateOnly month)
    {
        return await _dbContext
                     .Expenses
                     .AsNoTracking()
                     .Where(exp => exp.Date.Month.Equals(month.Month) && exp.Date.Year.Equals(month.Year))
                     .OrderBy(exp => exp.Date)
                     .ThenBy(exp => exp.Title)
                     .ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id)
    {
        return await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user, long id)
    {
        return await _dbContext.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
    }

    public async Task<bool> DeleteById(long id)
    {
        var registeredExpense = await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id.Equals(id));
        if (registeredExpense is null)
            return false;

        _dbContext.Expenses.Remove(registeredExpense);
        return true;
    }
}
