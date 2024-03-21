using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NetSchool.Web.Services.PdfGenerator;

public static class Bootstrapper
{
    public static IServiceCollection AddPdfGenerator(this IServiceCollection services, IConfiguration configuration = null)
    {
        services.AddSingleton(typeof(PdfGenerator));

        return services;
    }
}
