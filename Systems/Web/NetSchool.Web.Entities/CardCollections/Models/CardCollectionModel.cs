namespace NetSchool.Web.Entities.CardCollections;

public class CardCollectionModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public IEnumerable<CardModel> Cards { get; set; }
    public DateTime TimeExpiraton { get; set; }
}
