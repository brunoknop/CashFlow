using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.UsersRepositories;

public interface IUserUpdateOnlyRepository
{
    Task<User> GetById(long userId);
    
    void Update(User user);
}
