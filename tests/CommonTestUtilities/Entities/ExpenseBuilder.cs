using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using TagEntity = CashFlow.Domain.Entities.Tag;
using TagEnum = CashFlow.Domain.Enums.Tag;

namespace CommonTestUtilities.Entities;

public class ExpenseBuilder
{
    public static Expense Build(User user)
    {
        var expense = new Faker<Expense>()
                      .RuleFor(expense => expense.Id, _ => 1)
                      .RuleFor(expense => expense.Title, faker => faker.Commerce.ProductName())
                      .RuleFor(expense => expense.User, _ => user)
                      .RuleFor(expense => expense.UserId, _ => user.Id)
                      .RuleFor(expense => expense.Description, faker => faker.Commerce.ProductDescription())
                      .RuleFor(expense => expense.Amount, faker => faker.Random.Decimal(1, 1000))
                      .RuleFor(expense => expense.PaymentType, faker => faker.PickRandom<PaymentType>())
                      .RuleFor(expense => expense.Date, faker => faker.Date.Past())
                      .RuleFor(expense => expense.Tags, faker => faker.Make(1, () => new TagEntity()
                      {
                          Id = 1,
                          ExpenseId = 1,
                          TagType = faker.PickRandom<TagEnum>()
                      }));
        
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
