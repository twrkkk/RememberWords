using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Geom;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NetSchool.Context.Entities;
using NetSchool.Services.PdfGenerator.Templates;
using System.Text;

namespace NetSchool.Services.PdfGenerator;

public class PdfGenerator
{
    public byte[] CardCollectionToPdf(CardCollection cardCollection)
    {
        var html = HtmlToPdfTemplates.GetHTMLCardCollection(cardCollection);

        string cssContent;
        using (var reader = new StreamReader(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Assets", "styles.css")))
        {
            cssContent = reader.ReadToEnd();
        }

        using (var ms = new MemoryStream())
        {
            using (var doc = new Document())
            {
                doc.AddTitle(cardCollection.Name);

                using (var writer = PdfWriter.GetInstance(doc, ms))
                {
                    doc.Open();

                    using (var msCss = new MemoryStream(Encoding.UTF8.GetBytes(cssContent)))
                    {
                        using (var msHtml = new MemoryStream(Encoding.UTF8.GetBytes((string)html)))
                        {
                            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                        }
                    }

                    doc.Close();
                }
            }

            return ms.ToArray();
        }
    }
}
