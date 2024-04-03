using Microsoft.Extensions.DependencyInjection;
using NetSchool.Services.CardCollections.CardCollections;
using NetSchool.Services.Settings;

namespace NetSchool.Services.CardCollections;

public static class Bootstrapper
{
    public static IServiceCollection AddCartCollectionService(this IServiceCollection services, RedisSettings redisSettings)
    {
        return services
            .AddScoped<ICartCollectionService, CartCollectionService>()
            .AddStackExchangeRedisCache(options => {
                options.Configuration = redisSettings.Url;
                options.InstanceName = "local";
            });
    }
}
