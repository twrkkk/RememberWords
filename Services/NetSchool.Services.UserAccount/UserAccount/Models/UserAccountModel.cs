namespace NetSchool.Services.UserAccount;

using AutoMapper;
using NetSchool.Context.Entities;
using NetSchool.Services.CardCollections;

public class UserAccountModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public IEnumerable<CardCollectionModel> CardCollections { get; set; }
    public DateTime? RegistrationDate { get; set; } 
}

public class UserAccountModelProfile : Profile
{
    public UserAccountModelProfile()
    {
        CreateMap<User, UserAccountModel>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
            .ForMember(d => d.CardCollections, o => o.MapFrom(s => s.CardCollections))
            .ForMember(d => d.RegistrationDate, o => o.MapFrom(s => s.RegistrationDate))
            ;
    }
}
