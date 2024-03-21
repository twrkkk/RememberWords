using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NetSchool.Services.LoadCustomAssembly;

public static class Bootstrapper
{
    public static IServiceCollection AddCustomAssemblyLoader(this IServiceCollection services, IConfiguration configuration = null)
    {
        var context = new CustomAssemblyLoadContext();
        string fullPathToDll = Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll");
        context.LoadUnmanagedLibrary(fullPathToDll);

        return services;
    }
}
