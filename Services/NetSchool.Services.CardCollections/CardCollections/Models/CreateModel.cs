using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetSchool.Context.Entities;
using NetSchool.Context;
using NetSchool.Services.CardCollections.Cards.Models;
using FluentValidation;

namespace NetSchool.Services.CardCollections;

public class CreateModel
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public IEnumerable<CreateCardModel> Cards { get; set; }
    public DateTime? CreationTime { get; set; }
}

public class CreateModelProfile : Profile
{
    public CreateModelProfile()
    {
        CreateMap<CreateModel, CardCollection>()
            .BeforeMap<CardCollectionActions>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.TimeExpiration, opt => opt.MapFrom(src => src.CreationTime))
            ;
    }

    public class CardCollectionActions : IMappingAction<CreateModel, CardCollection>
    {
        private readonly IDbContextFactory<MainDbContext> contextFactory;

        public CardCollectionActions(IDbContextFactory<MainDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Process(CreateModel source, CardCollection destination, ResolutionContext context)
        {
            using var db = contextFactory.CreateDbContext();

            var user = db.Users.Include(x => x.CardCollections).FirstOrDefault(x => x.Id == source.UserId);

            destination.User = user;
        }
    }
}

public class CreateModelValidator : AbstractValidator<CreateModel>
{
    public CreateModelValidator(IDbContextFactory<MainDbContext> contextFactory)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
            .MinimumLength(1).WithMessage("Minimum length is 1")
            .MaximumLength(100).WithMessage("Maximum length is 100");

        RuleFor(x => x.Cards)
            .NotEmpty().WithMessage("At least one card must be");

        RuleFor(x => x.UserId).Must((id) =>
            {
                using var context = contextFactory.CreateDbContext();
                var found = context.Users.Any(a => a.Id == id);
                return found;
            }).WithMessage("User not found");
    }
}