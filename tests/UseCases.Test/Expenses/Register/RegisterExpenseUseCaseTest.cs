using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Repositories.Expenses;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using FluentAssertions;

namespace UseCases.Test.Expenses.Register;

public class RegisterExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterExpanseJsonBuilder.Build();
        var user = UserBuilder.Build();
        
        var useCase = CreateUseCase(user);

        var response = await useCase.Execute(request);

        response.Should().NotBeNull();
        response.Title.Should().Be(request.Title);
    }
    
    [Fact]
    public async Task Error_Title_Empty()
    {
        var request = RequestRegisterExpanseJsonBuilder.Build();
        request.Title = string.Empty;
        
        var user = UserBuilder.Build();
        
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var response = await act.Should().ThrowAsync<ErrorOnValidationException>();

        response.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }
    
    private RegisterExpenseUseCase CreateUseCase(User user)
    {
        var writeOnlyRepository = ExpensesWriteOnlyRepositoryBuilder.Build();
        var mapper = MapperBuilder.Build();
        var unityOfWork = UnityOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        
        return new RegisterExpenseUseCase(writeOnlyRepository, unityOfWork, mapper, loggedUser);
    }
}
