using NetSchool.Web.Entities.CardCollections;
using System.Text;

namespace NetSchool.Web.Services.PdfGenerator.Templates;

public class HtmlToPdfTemplates
{
    public static string GetHTMLCardCollection(CardCollectionModel cardCollection)
    {
        var cards = cardCollection.Cards;
        var sb = new StringBuilder();
        sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Memorizing</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Term</th>
                                        <th>Definition</th>
                                    </tr>");
        foreach (var card in cards)
        {
            sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                  </tr>", card.Front, card.Reverse);
        }
        sb.Append(@"
                                </table>
                            </body>
                        </html>");
        return sb.ToString();
    }
}
