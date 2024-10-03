using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.UsersRepositories;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class UsersRepository : IUserWriteOnlyRepository, IUserReadOnlyRespoitory, IUserUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;

    public UsersRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task Delete(User user)
    {
        var registeredUser = await _dbContext.Users.FindAsync(user.Id);
        _dbContext.Users.Remove(registeredUser!);
    }

    public async Task<User> GetById(long userId)
    {
        return await _dbContext.Users.FirstAsync(user => user.Id.Equals(userId));
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }

    public async Task<bool> ExistsActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }
}
