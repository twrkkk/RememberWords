namespace NetSchool.Context.Entities;

public class User: BaseEntity
{
    public string NickName { get; set; }
    public UserStatus Status { get; set; }
    public virtual ICollection<CardCollection> CardCollections { get; set; }
    //public DateTime? RegistratonDate { get; set; }
}
