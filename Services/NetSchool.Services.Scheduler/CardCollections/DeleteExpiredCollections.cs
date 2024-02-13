using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using NetSchool.Context;
using NetSchool.Services.Logger;

namespace NetSchool.Services.Scheduler.CardCollections;

public class DeleteExpiredCollections : IInvocable
{
    private readonly IDbContextFactory<MainDbContext> _dbContextFactory;
    private readonly IAppLogger _logger;

    public DeleteExpiredCollections(IDbContextFactory<MainDbContext> dbContextFactory, IAppLogger logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task Invoke()
    {
        _logger.Information("Delete Expired Collections Scheduler has started");

        using var context = await _dbContextFactory.CreateDbContextAsync();

        await context.Database.ExecuteSqlRawAsync("DELETE FROM card_collections AS c WHERE c.\"TimeExpiration\" <= CURRENT_TIMESTAMP");

        _logger.Information("Delete Expired Collections Scheduler has stopped");
    }
}
