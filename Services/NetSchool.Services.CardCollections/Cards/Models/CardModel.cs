using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetSchool.Context.Entities;
using NetSchool.Context;

namespace NetSchool.Services.CardCollections.Cards.Models;

public class CardModel
{
    public Guid Id { get; set; }
    public string Front { get; set; }
    public string Reverse { get; set; }
}

public class CardModelProfile : Profile
{
    public CardModelProfile()
    {
        CreateMap<CardModel, Card>()
            .BeforeMap<CardModelActions>()
            .ForMember(dest => dest.CardCollection, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Uid, opt => opt.Ignore())
            ;
    }

    public class CardModelActions : IMappingAction<CardModel, Card>
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;

        public CardModelActions(IDbContextFactory<MainDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Process(CardModel source, Card destination, ResolutionContext context)
        {
            using var db = contextFactory.CreateDbContext();

            destination.Uid = source.Id;
        }
    }
}
