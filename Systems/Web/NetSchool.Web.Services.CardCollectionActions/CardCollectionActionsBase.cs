using NetSchool.Web.Entities.CardCollections;
using NetSchool.Web.Entities.CardCollections.Enums;
using NetSchool.Web.Services.CardCollections;

namespace NetSchool.Web.Services.CardCollectionActions;

public abstract class CardCollectionActionsBase
{
    protected CardCollectionActionsBase(ICardCollectionsService cardService)
    {
        collection = new();
        collection.Cards = new List<CardModel>();
        this.cardService = cardService;
    }

    public CardCollectionModel collection { get; protected set; }
    protected readonly ICardCollectionsService cardService;

    public virtual void AddCard(CardModel model)
    {
        collection.Cards.Add(model);
    }

    public virtual void AddCards(CardModel[] models)
    {
        foreach (var model in models)
            AddCard(model);
    }

    public virtual void EditCard(CardModel newModel)
    {
        var card = collection.Cards.FirstOrDefault(x=>x.Id == newModel.Id);

        if (card == null)
            return;

        card.Front = newModel.Front;
        card.Reverse = newModel.Reverse;
    }

    public virtual void DeleteCard(Guid cardId)
    {
        var card = collection.Cards.FirstOrDefault(x => x.Id == cardId);

        if (card == null)
            return;

        collection.Cards.Remove(card);
    }

    public abstract Task SaveChanges(Guid collectionId, CardCollectionSavePeriod SavePeriod);
    public async Task GenerateWithAI(string prompt)
    {
        var result = await cardService.GenerateWithAI(prompt);

        collection.Cards = result.Select(x => new CardModel
        {
            Front = x.Front,
            Reverse = x.Reverse,
            Id = Guid.NewGuid()
        }).ToList();
    }
}
