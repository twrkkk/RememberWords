namespace NetSchool.Context.Seeder;

using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetSchool.Context.Entities;
using NetSchool.Services.UserAccount;
using System;

public static class DbSeeder
{
    private static IServiceScope ServiceScope(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetService<IServiceScopeFactory>()!.CreateScope();
    }

    private static MainDbContext DbContext(IServiceProvider serviceProvider)
    {
        return ServiceScope(serviceProvider)
            .ServiceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>().CreateDbContext();
    }

    public static void Execute(IServiceProvider serviceProvider)
    {
        Task.Run(async () =>
            {
                await AddAdministrator(serviceProvider);
                await AddDemoData(serviceProvider);
            })
            .GetAwaiter()
            .GetResult();
    }

    private static async Task AddDemoData(IServiceProvider serviceProvider)
    {
        using var scope = ServiceScope(serviceProvider);
        if (scope == null)
            return;

        var settings = scope.ServiceProvider.GetService<DbSettings>();
        if (!(settings.Init?.AddDemoData ?? false))
            return;

        await using var context = DbContext(serviceProvider);

        if (await context.CardCollections.AnyAsync())
            return;

        var user = DemoHelper.GetUser;
        var cardCollections = DemoHelper.GetCardCollections;

        user.CardCollections = cardCollections.ToList();

        await context.CardCollections.AddRangeAsync(cardCollections);
        await context.Users.AddAsync(user);

        await context.SaveChangesAsync();
    }

    private static async Task AddAdministrator(IServiceProvider serviceProvider)
    {
        using var scope = ServiceScope(serviceProvider);
        if (scope == null)
            return;

        var settings = scope.ServiceProvider.GetService<DbSettings>();
        if (!(settings.Init?.AddAdministrator ?? false))
            return;

        var userAccountService = scope.ServiceProvider.GetService<IUserAccountService>();

        if (!(await userAccountService.IsEmptyAsync()))
            return;

        await userAccountService.CreateAsync(new RegisterUserAccountModel()
        {
            UserName = settings.Init.Administrator.Email,
            Email = settings.Init.Administrator.Email,
            Password = settings.Init.Administrator.Password,
        });
    }
}