using CashFlow.Communication.Responses.Users;

namespace CashFlow.Application.UseCases.Users.GetProfile;

public interface IGetUserProfileUseCase
{
    Task<ResponseProfileUserJson> Execute();
}
