namespace NetSchool.Web.Entities.CardCollections;

public class UpdateModel
{
    public string Name { get; set; }
    public IList<CardModel> Cards { get; set; }
}
