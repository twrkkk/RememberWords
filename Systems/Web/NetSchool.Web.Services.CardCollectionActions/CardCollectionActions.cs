﻿using NetSchool.Web.Entities.CardCollections;

namespace NetSchool.Web.Services.CardCollectionActions;

public abstract class CardCollectionActions
{
    protected CardCollectionActions()
    {
        collection = new();
        collection.Cards = new List<CardModel>();
    }

    public CardCollectionModel collection { get; private set; }

    public virtual void AddCard(CardModel model)
    {
        collection.Cards.Add(model);
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

    public abstract Task SaveChanges(Guid collectionId);
}