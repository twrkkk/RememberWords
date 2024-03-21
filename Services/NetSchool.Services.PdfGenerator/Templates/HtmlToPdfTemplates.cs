using NetSchool.Context.Entities;
using System.Text;

namespace NetSchool.Services.PdfGenerator.Templates;

public class HtmlToPdfTemplates
{
    public static string GetHTMLCardCollection(CardCollection cardCollection)
    {
        var cards = cardCollection.Cards;
        var sb = new StringBuilder();
        sb.Append($@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>{cardCollection.Name}</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th style=""width: 50%;"">Term</th>
                                        <th style=""width: 50%;"">Definition</th>
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
