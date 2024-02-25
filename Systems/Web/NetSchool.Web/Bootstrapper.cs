using Microsoft.Extensions.DependencyInjection;
using NetSchool.Web.Services.Configuration;
using NetSchool.Web.Services.CardCollections;
using Microsoft.AspNetCore.Components.Authorization;
using NetSchool.Web.Providers;
using NetSchool.Web.Pages.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using NetSchool.Web.Pages.Registration.Services;

namespace NetSchool.Web;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddWebConfigurationService()
            .AddCardCollectionsService()
            .AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<IRegistrationService, RegistrationService>()
        ;

        return services;
    }
}