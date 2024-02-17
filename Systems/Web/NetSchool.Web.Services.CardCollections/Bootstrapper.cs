using Microsoft.Extensions.DependencyInjection;

namespace NetSchool.Web.Services.CardCollections;

public static class Bootstrapper
{
    public static IServiceCollection AddCardCollectionsService(this IServiceCollection services)
    {
        services.AddScoped<ICardCollectionsService, CardCollectionService>();

        return services;
    }
}
