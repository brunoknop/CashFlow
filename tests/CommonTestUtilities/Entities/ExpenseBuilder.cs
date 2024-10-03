using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class ExpenseBuilder
{
    public static Expense Build(User user)
    {
        var expense = new Faker<Expense>()
                      .RuleFor(u => u.Id, _ => 1)
                      .RuleFor(u => u.Title, faker => faker.Commerce.ProductName())
                      .RuleFor(u => u.User, _ => user)
                      .RuleFor(u => u.UserId, _ => user.Id)
                      .RuleFor(u => u.Description, faker => faker.Commerce.ProductDescription())
                      .RuleFor(u => u.Amount, faker => faker.Random.Decimal(1, 1000))
                      .RuleFor(u => u.PaymentType, faker => faker.PickRandom<PaymentType>())
                      .RuleFor(u => u.Date, faker => faker.Date.Past());
        
        return expense;
    }

    public static List<Expense> Colletion(User user, uint? quantity = 3)
    {
        if (quantity is 0) quantity = 2;
        
        var expenses = new List<Expense>();
        
        for (var i = 1; i <= quantity; i++)
        {
            var expense = Build(user);
            expense.Id = i;
            expenses.Add(expense);
        }
        
        return expenses;
    }
}
