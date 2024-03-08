using Microsoft.AspNetCore.Identity;

namespace NetSchool.Context.Entities;

public class User : IdentityUser<Guid>
{
    public UserStatus Status { get; set; }
    public virtual ICollection<CardCollection> CardCollections { get; set; }
    public virtual ICollection<User> Followers { get; set; }
    public virtual ICollection<User> Following { get; set; }
    public DateTime? RegistrationDate { get; set; }
}
