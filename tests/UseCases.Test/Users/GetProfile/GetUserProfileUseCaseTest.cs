using CashFlow.Application.UseCases.Users.GetProfile;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Services.LoggedUser;
using FluentAssertions;

namespace UseCases.Test.Users.GetProfile;

public class GetUserProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        
        var useCase = CreateUseCase(user);
        
        var result = await useCase.Execute();
        
        result.Email.Should().Be(user.Email);
        result.Name.Should().Be(user.Name);
    }
    
    private GetUserProfileUseCase CreateUseCase(User user)
    {
        var logger = LoggedUserBuilder.Build(user);
        var mapper = MapperBuilder.Build();
        return new GetUserProfileUseCase(logger, mapper);
    }
}
