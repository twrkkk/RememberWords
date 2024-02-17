using Microsoft.Extensions.DependencyInjection;

namespace NetSchool.Web.Services.Configuration;

public static class Bootstrapper
{
    public static IServiceCollection AddWebConfigurationService(this IServiceCollection services)
    {
        services.AddScoped<IConfigurationService, ConfigurationService>();

        return services;
    }
}
