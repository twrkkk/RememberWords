using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NetSchool.Context;
using NetSchool.Services.CardCollections.Cards.Models;
using NetSchool.Services.CardCollections.Enums;

namespace NetSchool.Services.CardCollections;

public class UpdateModel
{
    public string Name { get; set; }
    public IList<CardModel> UpdatedCards { get; set; }
    public IList<Guid> DeletedCardsId { get; set; }
    public CardCollectionSavePeriod SavePeriod { get; set; }
}
public class UpdateModelValidator : AbstractValidator<UpdateModel>
{
    public UpdateModelValidator(IDbContextFactory<MainDbContext> contextFactory)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
            .MinimumLength(1).WithMessage("Minimum length is 1")
            .MaximumLength(100).WithMessage("Maximum length is 100");
    }
}