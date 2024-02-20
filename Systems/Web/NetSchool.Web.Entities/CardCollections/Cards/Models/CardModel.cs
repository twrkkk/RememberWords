namespace NetSchool.Web.Entities.CardCollections;

public class CardModel
{
    public Guid Id { get; set; }
    public string Front { get; set; }
    public string Reverse { get; set; }

    public override bool Equals(object obj)
    {
        return (obj is CardModel card) &&
            card.Id == Id;
    }
}