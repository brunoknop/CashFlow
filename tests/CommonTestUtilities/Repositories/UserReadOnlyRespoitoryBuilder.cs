using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.UsersRepositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRespoitoryBuilder
{
    private readonly Mock<IUserReadOnlyRespoitory> _repository;

    public UserReadOnlyRespoitoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRespoitory>();
    }

    public void ExistsActiveUserWithEmail(string email)
    {
        _repository.Setup(userReadOnly => userReadOnly.ExistsActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public UserReadOnlyRespoitoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(userReadOnly => userReadOnly.GetUserByEmail(user.Email)).ReturnsAsync(user);
        return this;
    }
    
    public IUserReadOnlyRespoitory Build() => _repository.Object;
}
