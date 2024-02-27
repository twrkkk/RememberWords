using Microsoft.AspNetCore.Components.Authorization;
using NetSchool.Web.Entities.CardCollections;
using NetSchool.Web.Services.CardCollections;

namespace NetSchool.Web.Services.CardCollectionActions
{
    public class CreateCardCollectionActions : CardCollectionActions
    {
        private readonly AuthenticationStateProvider authProvider;
        private readonly ICardCollectionsService cardService;

        public CreateCardCollectionActions(AuthenticationStateProvider authProvider, ICardCollectionsService cardService)
        {
            this.authProvider = authProvider;
            this.cardService = cardService;
        }

        public override async Task SaveChanges(Guid collectionId)
        {
            var authState = await authProvider.GetAuthenticationStateAsync();
            var userId = authState.User.Claims.FirstOrDefault(x => x.Type.ToLower() == "sub").Value;
            var createdCollection = new CreateModel
            {
                Name = collection.Name,
                UserId = new Guid(userId),
                Cards = collection.Cards.Select(x => new CreateCardModel { Front = x.Front, Reverse = x.Reverse }).ToList()
            };

            await cardService.Create(createdCollection);
        }
    }
}
