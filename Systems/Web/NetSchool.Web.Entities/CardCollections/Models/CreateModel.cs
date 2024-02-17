﻿namespace NetSchool.Web.Entities.CardCollections;

public class CreateModel
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public IEnumerable<CreateCardModel> Cards { get; set; }
    public DateTime? CreationTime { get; set; }
}