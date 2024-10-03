using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.UsersRepositories;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.Delete;

public class DeleteUserUseCase(
    ILoggedUser loggedUser,
    IUserWriteOnlyRepository writeRepository,
    IUnityOfWork unityOfWork
) : IDeleteUserUseCase
{
    public async Task Execute()
    {
        var user = await loggedUser.GetLoggedUser();
        
        await writeRepository.Delete(user);
        await unityOfWork.Commit();
    }
}
