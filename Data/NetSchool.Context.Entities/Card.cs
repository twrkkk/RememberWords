﻿namespace NetSchool.Context.Entities;

public class Card : BaseEntity
{
    public string Front { get; set; }
    public string Reverse { get; set; }
    public virtual CardCollection CardCollection { get; set; }

    public override bool Equals(object obj)
    {
        return (obj is Card card) &&
            card.Uid == Uid;
    }
}
