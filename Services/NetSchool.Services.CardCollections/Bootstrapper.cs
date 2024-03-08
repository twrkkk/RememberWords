using Microsoft.Extensions.DependencyInjection;
using NetSchool.Services.CardCollections.CardCollections;

namespace NetSchool.Services.CardCollections
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddCartCollectionService(this IServiceCollection services)
        {
            return services
                .AddScoped<ICartCollectionService, CartCollectionService>();
        }
    }
}
