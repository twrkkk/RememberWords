using NetSchool.Web.Entities.CardCollections;
using NetSchool.Web.Entities.CardCollections.Enums;
using NetSchool.Web.Services.CardCollections;

namespace NetSchool.Web.Services.CardCollectionActions
{
    public class UpdateCardCollectionActions : CardCollectionActionsBase
    {
        private readonly ICardCollectionsService cardService;
        private UpdateModel updateModel;

        public UpdateCardCollectionActions(ICardCollectionsService cardService)
        {
            this.cardService = cardService;
            updateModel = new();
            updateModel.UpdatedCards = new List<CardModel>();
            updateModel.DeletedCardsId = new List<Guid>();
        }

        public override void AddCard(CardModel model)
        {
            base.AddCard(model);

            updateModel.UpdatedCards.Add(model);
        }

        public override void EditCard(CardModel newModel)
        {
            base.EditCard(newModel);

            var existCard = updateModel.UpdatedCards.FirstOrDefault(x => x.Id == newModel.Id);
            if (existCard == null) // changing an existing card
            {
                updateModel.UpdatedCards.Add(newModel);
            }
            else // changing the created card
            {
                existCard.Front = newModel.Front;
                existCard.Reverse = newModel.Reverse;
            }
        }

        public override void DeleteCard(Guid cardId)
        {
            base.DeleteCard(cardId);

            var cardToDelete = updateModel.UpdatedCards.FirstOrDefault(x => x.Id == cardId);

            if (cardToDelete != null)
            {
                updateModel.UpdatedCards.Remove(cardToDelete);
            }

            updateModel.DeletedCardsId.Add(cardId);
        }

        public override async Task SaveChanges(Guid collectionId, CardCollectionSavePeriod savePeriod)
        {
            updateModel.Name = collection.Name;
            updateModel.SavePeriod = savePeriod;
            try
            {
                await cardService.Update(collectionId, updateModel);
            }
            catch
            {
                throw;
            }
        }
    }
}
