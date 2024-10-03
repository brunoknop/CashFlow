using CashFlow.Communication.Requests.Users;

namespace CashFlow.Application.UseCases.Users.Update.Profile;

public interface IUpdateUserProfileUseCase
{
    Task Execute(RequestUpdateUserProfileJson request);
}
