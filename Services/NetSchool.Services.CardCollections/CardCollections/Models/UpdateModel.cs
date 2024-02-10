using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NetSchool.Context;
using NetSchool.Services.CardCollections.Cards.Models;

namespace NetSchool.Services.CardCollections;

public class UpdateModel
{
    public string Name { get; set; }
    public IEnumerable<CardModel> Cards { get; set; }
}

public class UpdateModelValidator : AbstractValidator<UpdateModel>
{
    public UpdateModelValidator(IDbContextFactory<MainDbContext> contextFactory)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
            .MinimumLength(1).WithMessage("Minimum length is 1")
            .MaximumLength(100).WithMessage("Maximum length is 100");

        RuleFor(x => x.Cards)
            .NotEmpty().WithMessage("At least one card must be");
    }
}