using CashFlow.Communication.Enum;

namespace CashFlow.Communication.Requests.Expenses;

public class RequestExpanseJson
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public PaymentType PaymentType { get; set; }

    public List<Tag> Tags { get; set; } = [];
}
