namespace NetSchool.Web.Entities.CardCollections;

public class CardModel
{
    public Guid Id { get; set; }
    public string Front { get; set; }
    public string Reverse { get; set; }

    // SM-2 поля
    public int Repetition { get; set; } = 0;
    public int Interval { get; set; } = 0;
    public double Easiness { get; set; } = 2.5;
    public DateTime NextReviewDate { get; set; } = DateTime.Today;

    // Служебное
    public bool IsFlipped { get; set; } = false;
    public bool IsAnswerRevealed { get; set; } = false;
    public bool IsFrontVisible { get; set; } = true;


    public override bool Equals(object obj)
    {
        return (obj is CardModel card) &&
            card.Id == Id;
    }
}