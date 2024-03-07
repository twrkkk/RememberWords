using NetSchool.Web.Entities.CardCollections;

namespace NetSchool.Web.Entities.User;

public class UserAccountModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public IEnumerable<CardCollectionModel> CardCollections { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public bool EmailConfirmed {  get; set; }   
}
