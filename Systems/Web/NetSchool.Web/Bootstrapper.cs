using Microsoft.Extensions.DependencyInjection;
using NetSchool.Web.Services.Configuration;
using NetSchool.Web.Services.CardCollections;

namespace NetSchool.Web;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddWebConfigurationService()
            .AddCardCollectionsService()
            ;

        return services;
    }
}