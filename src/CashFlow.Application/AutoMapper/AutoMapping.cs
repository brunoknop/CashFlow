using AutoMapper;
using CashFlow.Communication.Requests.Expenses;
using CashFlow.Communication.Requests.Users;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Communication.Responses.Users;
using CashFlow.Domain.Entities;
using Tag = CashFlow.Communication.Enum.Tag;

namespace CashFlow.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore());

        
        CreateMap<RequestExpanseJson, Expense>()
            .ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Distinct()));
        
        
        CreateMap<Tag, Domain.Entities.Tag>()
            .ForMember(dest => dest.TagType, config => config.MapFrom(source => source));
    }

    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseRegisteredExpenseJson>();
        CreateMap<Expense, ResponseShortExpenseJson>();
        CreateMap<Expense, ResponseExpenseJson>()
            .ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Select(tag => tag.TagType)));

        CreateMap<User, ResponseRegisteredUserJson>();
        CreateMap<User, ResponseProfileUserJson>();
    }
}
