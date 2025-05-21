using NetSchool.Services.Settings;
using NetSchool.Services.Logger;
using NetSchool.Services.CardCollections;
using NetSchool.Context.Seeder;
using NetSchool.Services.UserAccount;
using NetSchool.Api.Settings;
using NetSchool.Services.RabbitMq;
using NetSchool.Services.Actions;
using NetSchool.Services.PdfGenerator;
using NetSchool.Services.Settings.Settings;

namespace NetSchool.Api;

public static class Bootstrapper
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        var redisSettings = NetSchool.Settings.Settings.Load<RedisSettings>("Redis");
        var YandexSettings = NetSchool.Settings.Settings.Load<YandexGPTSettings>("YandexGPT");

        services.AddMainSettings()
            .AddSwaggerSettings()
            .AddLogSettings()
            .AddAppLogger()
            .AddCartCollectionService(redisSettings, YandexSettings)
            .AddIdentitySettings()
            .AddDbSeeder()
            .AddUserAccountService()
            .AddApiSpecialSettings()
            .AddRabbitMq()
            .AddActions()
            .AddPdfGenerator()
            //.AddCustomAssemblyLoader()
            ;

        return services;
    }
}
