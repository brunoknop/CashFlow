namespace CashFlow.Communication.Requests.Users;

public class RequestUpdateUserPasswordJson
{
    public string CurrentPassword { get; set; } = string.Empty;
    
    public string NewPassword { get; set; } = string.Empty;
}
