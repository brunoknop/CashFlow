using AutoMapper;
using CashFlow.Communication.Responses.Users;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.GetProfile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }
    
    public async Task<ResponseProfileUserJson> Execute()
    {
        var loggedUser  = await _loggedUser.GetLoggedUser();
        return _mapper.Map<ResponseProfileUserJson>(loggedUser);
    }
}
