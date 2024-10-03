using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.UsersRepositories;

public interface IUserWriteOnlyRepository
{
    Task Add(User user);
    
    Task Delete(User user);
}
