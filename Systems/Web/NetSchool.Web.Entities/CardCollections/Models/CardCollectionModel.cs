namespace NetSchool.Web.Entities.CardCollections;

public class CardCollectionModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public IList<CardModel> Cards { get; set; }
    public DateTime TimeExpiraton { get; set; }

    public CardCollectionModel()
    {
        Cards = new List<CardModel>();
        Name = string.Empty;
    }
}
