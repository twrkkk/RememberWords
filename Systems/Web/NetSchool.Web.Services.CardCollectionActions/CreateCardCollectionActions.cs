﻿using Microsoft.AspNetCore.Components.Authorization;
using NetSchool.Web.Entities.CardCollections;
using NetSchool.Web.Entities.CardCollections.Enums;
using NetSchool.Web.Services.CardCollections;

namespace NetSchool.Web.Services.CardCollectionActions
{
    public class CreateCardCollectionActions : CardCollectionActionsBase
    {
        private readonly AuthenticationStateProvider authProvider;

        public CreateCardCollectionActions(AuthenticationStateProvider authProvider, ICardCollectionsService cardService): base(cardService)
        {
            this.authProvider = authProvider;
        }

        public override async Task SaveChanges(Guid collectionId, CardCollectionSavePeriod SavePeriod)
        {
            var authState = await authProvider.GetAuthenticationStateAsync();
            var userId = authState.User.Claims.FirstOrDefault(x => x.Type.ToLower() == "sub").Value;
            var createdCollection = new CreateModel
            {
                Name = collection.Name,
                UserId = new Guid(userId),
                Cards = collection.Cards.Select(x => new CreateCardModel { Front = x.Front, Reverse = x.Reverse }).ToList(),
                SavePeriod = SavePeriod
            };

            try
            {
                await cardService.Create(createdCollection);
            }
            catch
            {
                throw;
            }
        }
    }
}
