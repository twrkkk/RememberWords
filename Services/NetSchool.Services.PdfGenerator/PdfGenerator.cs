using DinkToPdf;
using DinkToPdf.Contracts;
using NetSchool.Context.Entities;
using NetSchool.Services.PdfGenerator.Templates;

namespace NetSchool.Services.PdfGenerator;

public class PdfGenerator
{
    private readonly IConverter _converter;

    public PdfGenerator(IConverter converter)
    {
        _converter = converter;
    }

    public byte[] CardCollectionToPdf(CardCollection cardCollection)
    {
        var globalSettings = new GlobalSettings
        {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
            Margins = new MarginSettings { Top = 10 },
            DocumentTitle = cardCollection.Name
        };

        var objectSettings = new ObjectSettings
        {
            PagesCount = true,
            HtmlContent = HtmlToPdfTemplates.GetHTMLCardCollection(cardCollection),
            WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "styles.css") },
            HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = false },
        };
        var pdf = new HtmlToPdfDocument()
        {
            GlobalSettings = globalSettings,
            Objects = { objectSettings }
        };

        var file = _converter.Convert(pdf);
        return file;
    }
}
