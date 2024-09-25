using CashFlow.Communication.Requests.Users;
using CashFlow.Communication.Responses.Users;

namespace CashFlow.Application.UseCases.Users.Login;

public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
