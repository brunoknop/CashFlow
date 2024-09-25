using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.UsersRepositories;

public interface IUserReadOnlyRespoitory
{
    Task<bool> ExistsActiveUserWithEmail(string email);
    
    Task<User?> GetUserByEmail(string email);
}
