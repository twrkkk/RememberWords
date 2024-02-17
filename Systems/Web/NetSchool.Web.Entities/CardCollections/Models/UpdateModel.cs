namespace NetSchool.Web.Entities.CardCollections;

public class UpdateModel
{
    public string Name { get; set; }
    public IEnumerable<CardModel> Cards { get; set; }
}
