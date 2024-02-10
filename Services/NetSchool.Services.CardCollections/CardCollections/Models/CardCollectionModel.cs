using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetSchool.Context.Entities;
using NetSchool.Context;
using NetSchool.Services.CardCollections.Cards.Models;

namespace NetSchool.Services.CardCollections;

public class CardCollectionModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public IEnumerable<CardModel> Cards { get; set; }
    public DateTime TimeExpiraton { get; set; }
}

public class CardCollectionModelProfile : Profile
{
    public CardCollectionModelProfile()
    {
        CreateMap<CardCollection, CardCollectionModel>()
            .BeforeMap<CardCollectionActions>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Cards, opt => opt.Ignore())
            .ForMember(dest => dest.TimeExpiraton, opt => opt.MapFrom(src => src.TimeExpiration))
            ;
    }

    public class CardCollectionActions : IMappingAction<CardCollection, CardCollectionModel>
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;

        public CardCollectionActions(IDbContextFactory<MainDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Process(CardCollection source, CardCollectionModel destination, ResolutionContext context)
        {
            using var db = contextFactory.CreateDbContext();

            var collection = db.CardCollections.Include(x => x.User).Include(x => x.Cards).FirstOrDefault(x => x.Id == source.Id);

            destination.Id = collection.Uid;
            destination.UserId = collection.User.Uid;
            destination.Cards = source.Cards.Select(x => new CardModel { Id = x.Uid, Front = x.Front, Reverse = x.Reverse });
        }
    }
}