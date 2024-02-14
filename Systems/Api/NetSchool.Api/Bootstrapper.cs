using NetSchool.Services.Settings;
using NetSchool.Services.Logger;
using NetSchool.Services.CardCollections;
using NetSchool.Context.Seeder;
using NetSchool.Services.UserAccount;
using NetSchool.Api.Settings;

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
            .AddIdentitySettings()
            .AddDbSeeder()
            .AddUserAccountService()
            .AddApiSpecialSettings()
            ;

        return services;
    }
}
