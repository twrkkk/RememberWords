namespace NetSchool.Context.Entities;

public class CardCollection : BaseEntity
{
    public int? UserId { get; set; }
    public virtual User User { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Card> Cards { get; set; }
    public DateTime? TimeExpiration { get; set; }
}
