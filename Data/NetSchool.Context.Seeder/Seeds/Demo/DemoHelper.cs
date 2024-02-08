﻿namespace NetSchool.Context.Seeder;

using NetSchool.Context.Entities;

public class DemoHelper
{
    public IEnumerable<CardCollection> GetCardCollections = new List<CardCollection>
    {
        new CardCollection()
        {
            Name = "test name",
            TimeExpiration = DateTime.UtcNow.AddDays(7),
            Cards = new List<Card>()
            {
                new Card
                {
                    Front = "front 1",
                    Reverse = "reverse 1"
                },
                new Card
                {
                    Front = "front 2",
                    Reverse = "reverse 2"
                },
                new Card
                {
                    Front = "front 3",
                    Reverse = "reverse 3"
                }
            }
        },
        new CardCollection()
        {
            Name = "test name 2",
            TimeExpiration = DateTime.UtcNow.AddDays(7),
            Cards = new List<Card>()
            {
                new Card
                {
                    Front = "front 1",
                    Reverse = "reverse 1"
                },
                new Card
                {
                    Front = "front 2",
                    Reverse = "reverse 2"
                },
                new Card
                {
                    Front = "front 3",
                    Reverse = "reverse 3"
                }
            }
        }
    };
}