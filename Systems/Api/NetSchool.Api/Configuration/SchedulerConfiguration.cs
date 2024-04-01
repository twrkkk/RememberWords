using Coravel;
using NetSchool.Services.Scheduler.CardCollections;
using NetSchool.Services.Settings;

namespace NetSchool.Api.Configuration;

public static class SchedulerConfiguration
{
    public static IServiceCollection AddAppDeleteExpiredCollectionsScheduler(this IServiceCollection services)
    {
        return services
            .AddSingleton<DeleteExpiredCollections>();
    }

    public static void UseAppDeleteExpiredCollectionsScheduler(this WebApplication app)
    {
        var configuration = NetSchool.Settings.Settings.Load<SchedulerSettings>("SchedulerSettings");

        app.Services.UseScheduler(scheduler =>
        {
            scheduler.Schedule<DeleteExpiredCollections>()
            .EverySeconds(configuration.DeleteExpiredCollections.Frequency); //.EveryTenMinutes();
        });
    }
}
