using NetSchool.Web.Entities.CardCollections;
using NetSchool.Web.Services.PdfGenerator.Templates;

namespace NetSchool.Web.Services.PdfGenerator;

public class PdfGenerator
{
    public async Task<MemoryStream> CardCollectionToPdf(CardCollectionModel cardCollection)
    {
        var Renderer = new IronPdf.ChromePdfRenderer();
        Renderer.RenderingOptions.PaperSize = IronPdf.Rendering.PdfPaperSize.A2;

        var htmlString = HtmlToPdfTemplates.GetHTMLCardCollection(cardCollection);
        var doc = await Renderer.RenderHtmlAsPdfAsync(htmlString);

        return doc.Stream;
    }
}
