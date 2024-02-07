using NetSchool.Services.Settings;
using NetSchool.Services.Logger;

namespace NetSchool.Api;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddMainSettings()
            .AddSwaggerSettings()
            .AddLogSettings()
            .AddAppLogger();

        return services;
    }
}
