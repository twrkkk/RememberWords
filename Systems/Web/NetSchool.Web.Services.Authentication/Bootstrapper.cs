using Microsoft.Extensions.DependencyInjection;

namespace NetSchool.Web.Services.Authentication;

public static class Bootstrapper
{
    public static IServiceCollection AddCardCollectionsService(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
