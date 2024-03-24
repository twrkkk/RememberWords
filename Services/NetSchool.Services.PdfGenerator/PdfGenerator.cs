using Aspose.Pdf;
using Aspose.Pdf.Text;
using NetSchool.Context.Entities;

namespace NetSchool.Services.PdfGenerator
{
    public class PdfGenerator
    {
        public byte[] CardCollectionToPdf(CardCollection cardCollection)
        {
            Document doc = new Document();
            Page page = doc.Pages.Add();

            TextFragment textFragment = new TextFragment(cardCollection.Name);
            textFragment.TextState.FontSize = 14;
            textFragment.TextState.HorizontalAlignment = HorizontalAlignment.Center;
            page.Paragraphs.Add(textFragment);
            page.Paragraphs.Add(new TextFragment());

            Table table = new Table();
            table.Border = new BorderInfo(BorderSide.All, .5f, Color.FromRgb(System.Drawing.Color.LightGray));
            table.DefaultCellBorder = new BorderInfo(BorderSide.All, .5f, Color.FromRgb(System.Drawing.Color.LightGray));
            table.DefaultCellTextState = new TextState(12);
            table.DefaultCellTextState.HorizontalAlignment = HorizontalAlignment.Center;

            Row row = table.Rows.Add();
            row.Cells.Add("Term");
            row.Cells.Add("Definition");

            foreach (var card in cardCollection.Cards)
            {
                row = table.Rows.Add();
                row.Cells.Add(card.Front);
                row.Cells.Add(card.Reverse);
            }

            table.ColumnWidths = "50%";

            page.Paragraphs.Add(table);

            MemoryStream stream = new MemoryStream();
            doc.Save(stream);

            return stream.ToArray();
        }
    }
}
