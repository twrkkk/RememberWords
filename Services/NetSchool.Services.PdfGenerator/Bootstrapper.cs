using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NetSchool.Services.PdfGenerator;

public static class Bootstrapper
{
    public static IServiceCollection AddPdfGenerator(this IServiceCollection services, IConfiguration configuration = null)
    {
        services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        services.AddSingleton(typeof(PdfGenerator));

        return services;
    }
}
