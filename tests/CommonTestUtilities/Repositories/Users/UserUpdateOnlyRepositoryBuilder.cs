using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.UsersRepositories;
using Moq;

namespace CommonTestUtilities.Repositories.Users;

public class UserUpdateOnlyRepositoryBuilder
{
    public static IUserUpdateOnlyRepository Build(User? user = null)
    {
        var mock = new Mock<IUserUpdateOnlyRepository>();
        
        if(user is not null)
            mock.Setup(updateRepository => updateRepository.GetById(user.Id)).ReturnsAsync(user);
        
        return mock.Object;
    }
}
