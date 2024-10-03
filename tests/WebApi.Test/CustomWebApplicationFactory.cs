using CashFlow.Api;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public UserIdentityManager User_Team_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;
    public ExpenseIdentityManeger Team_Member_Expense { get; private set; } = default!;
    public ExpenseIdentityManeger Admin_Expense { get; private set; } = default!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
               .ConfigureServices(services =>
               {
                   var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                   services.AddDbContext<CashFlowDbContext>(config =>
                   {
                       config.UseInMemoryDatabase("InMemoryDbForTesting");
                       config.UseInternalServiceProvider(provider);
                   });

                   var scope = services.BuildServiceProvider().CreateScope();
                   var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                   var encrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();
                   var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                   StartDatabase(dbContext, encrypter, tokenGenerator);
               });
    }
    
    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncrypter encrypter, IAccessTokenGenerator tokenGenerator)
    {
        var userTeamMember = AddUserTeamMember(dbContext, encrypter, tokenGenerator);
        var userAdmin = AddUserAdmin(dbContext, encrypter, tokenGenerator);
        AddTeamMemberExpenses(dbContext, userTeamMember);
        AddAdminExpenses(dbContext, userAdmin);
    }

    private User AddUserTeamMember(CashFlowDbContext dbContext, IPasswordEncrypter encrypter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 0;
        var password = user.Password;
        
        user.Password = encrypter.Encrypt(user.Password);
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
        
        var token = tokenGenerator.Generate(user);
        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }
    
    private User AddUserAdmin(CashFlowDbContext dbContext, IPasswordEncrypter encrypter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build(Role.Administrator);
        user.Id = 0;
        var password = user.Password;
        
        user.Password = encrypter.Encrypt(user.Password);
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
        
        var token = tokenGenerator.Generate(user);
        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private void AddTeamMemberExpenses(CashFlowDbContext dbContext, User user)
    {
        var expenses = ExpenseBuilder.Colletion(user, 5);
        foreach(var expense in expenses)
        {
            expense.Id = 0;
            foreach(var tag in expense.Tags) tag.Id = 0;
        } 
        dbContext.Expenses.AddRange(expenses);
        dbContext.SaveChanges();
        
        Team_Member_Expense = new ExpenseIdentityManeger(expenses.FirstOrDefault()!);
    }
    
    private void AddAdminExpenses(CashFlowDbContext dbContext, User user)
    {
        var expenses = ExpenseBuilder.Colletion(user, 5);
        foreach(var expense in expenses)
        {
            expense.Id = 0;
            foreach(var tag in expense.Tags) tag.Id = 0;
        }
        dbContext.Expenses.AddRange(expenses);
        dbContext.SaveChanges();

        Admin_Expense = new ExpenseIdentityManeger(expenses.FirstOrDefault()!);
    }
}
