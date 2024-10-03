using CashFlow.Communication.Requests.Users;

namespace CashFlow.Application.UseCases.Users.Update.Password;

public interface IUpdateUserPasswordUseCase
{
    Task Execute(RequestUpdateUserPasswordJson request);
}
