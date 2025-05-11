using Microsoft.Extensions.DependencyInjection;
using NetSchool.Services.CardCollections.CardCollections;
using NetSchool.Services.Settings;

namespace NetSchool.Services.CardCollections;

public static class Bootstrapper
{
    public static IServiceCollection AddCartCollectionService(this IServiceCollection services, RedisSettings redisSettings)
    {
        services
            .AddScoped<ICartCollectionService, CartCollectionService>()
        .AddStackExchangeRedisCache(options => {
            options.Configuration = redisSettings.Url;
            options.InstanceName = "local";
        })
        .AddHttpClient("YandexGPT", c =>
        {
            c.BaseAddress = new Uri("https://llm.api.cloud.yandex.net");
            c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "someToken");
        });

        return services;
    }
}
