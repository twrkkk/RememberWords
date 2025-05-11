using System.Collections.Generic;

namespace NetSchool.Services.CardCollections.CardCollections.Models;

public class GenerateCollectionResponse
{
    public Result result { get; set; }

    public class Result
    {
        public List<Alternative> alternatives { get; set; }
    }

    public class Alternative
    {
        public Message message { get; set; }
        public string status { get; set; }
    }

    public class Message
    {
        public string text { get; set; }
    }
}
