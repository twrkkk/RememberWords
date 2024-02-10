using NetSchool.Services.Settings;
using NetSchool.Services.Logger;
using NetSchool.Services.CardCollections;

namespace NetSchool.Api;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddMainSettings()
            .AddSwaggerSettings()
            .AddLogSettings()
            .AddAppLogger()
            .AddCartCollectionService()
            ;

        return services;
    }
}
