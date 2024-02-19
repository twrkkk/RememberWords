namespace NetSchool.Web.Entities.CardCollections;

public class CreateModel
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public IList<CreateCardModel> Cards { get; set; }
}