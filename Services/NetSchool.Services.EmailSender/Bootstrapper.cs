using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetSchool.Services.EmailSender.Models;

namespace NetSchool.Services.EmailSender;

public static class Bootstrapper
{
    public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration = null)
    {
        var settings = Settings.Settings.Load<EmailConfiguration>("EmailConfiguration", configuration);
        services.AddSingleton(settings);

        services.AddSingleton<IEmailSender, EmailSender>();

        return services;
    }
}
