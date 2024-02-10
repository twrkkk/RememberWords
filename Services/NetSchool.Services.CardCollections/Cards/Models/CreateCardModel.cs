using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetSchool.Context.Entities;
using NetSchool.Context;

namespace NetSchool.Services.CardCollections.Cards.Models;

public class CreateCardModel
{
    public string Front {  get; set; }
    public string Reverse {  get; set; }
}

public class CreateCardModelProfile : Profile
{
    public CreateCardModelProfile()
    {
        CreateMap<CreateCardModel, Card>()
            .ForMember(dest => dest.CardCollection, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Uid, opt => opt.Ignore())
            ;
    }
}