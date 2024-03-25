using NetSchool.Web.Entities.CardCollections.Enums;

namespace NetSchool.Web.Entities.CardCollections;

public class UpdateModel
{
    public string Name { get; set; }
    public IList<CardModel> UpdatedCards { get; set; }
    public IList<Guid> DeletedCardsId { get; set; }
    public CardCollectionSavePeriod SavePeriod { get; set; }
}
