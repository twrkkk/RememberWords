using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace NetSchool.Services.Filters;

public static class Bootstrapper
{

    public static IServiceCollection AddFilters(this IServiceCollection services, IConfiguration configuration = null)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<CustomExceptionFilter>();
        });

        return services;
    }

}
