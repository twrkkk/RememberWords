namespace NetSchool.Context.Seeder;

using NetSchool.Context.Entities;

public static class DemoHelper
{
    public static User GetUser = new User
    {
        UserName = "bob228",
        Status = UserStatus.Active,
        RegistrationDate = DateTime.UtcNow,
    };

    public static IEnumerable<CardCollection> GetCardCollections = new List<CardCollection>
    {
        new CardCollection()
        {
            Name = "Продукты",
            TimeExpiration = DateTime.UtcNow.AddDays(7),
            Cards = new List<Card>()
            {
                new Card
                {
                    Front = "Apple",
                    Reverse = "Яблоко"
                },
                new Card
                {
                    Front = "Cheese",
                    Reverse = "Сыр"
                },
                new Card
                {
                    Front = "Potato",
                    Reverse = "Картофель"
                }
            }
        },
        new CardCollection()
        {
            Name = "Home",
            TimeExpiration = DateTime.UtcNow.AddDays(3),
            Cards = new List<Card>()
            {
                new Card
                {
                    Front = "Table",
                    Reverse = "Стол"
                },
                new Card
                {
                    Front = "Chair",
                    Reverse = "Стул"
                },
                new Card
                {
                    Front = "Window",
                    Reverse = "Окно"
                }
                ,
                new Card
                {
                    Front = "Sofa",
                    Reverse = "Диван"
                }
            }
        }
    };
}