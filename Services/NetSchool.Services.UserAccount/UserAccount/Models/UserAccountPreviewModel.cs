using AutoMapper;
using NetSchool.Context.Entities;

namespace NetSchool.Services.UserAccount.UserAccount.Models;

public class UserAccountPreviewModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
}

public class UserAccountPreviewModelProfile : Profile
{
    public UserAccountPreviewModelProfile()
    {
        CreateMap<User, UserAccountPreviewModel>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
            ;
    }
}
