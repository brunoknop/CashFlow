using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources;

public class ExpenseIdentityManeger
{
    private Expense _expense;

    public ExpenseIdentityManeger(Expense expense)
    {
        _expense = expense;
    }
    
    public long GetId() => _expense.Id;
    
    public DateTime GetDate() => _expense.Date;
}
