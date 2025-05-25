using Domain.Entities;
using Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundServices;

public sealed class RemoveExpiredEmailVerificationTokensBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RemoveExpiredEmailVerificationTokensBackgroundService> _logger;
    public RemoveExpiredEmailVerificationTokensBackgroundService(IServiceScopeFactory scopeFactory, ILogger<RemoveExpiredEmailVerificationTokensBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger
            .LogInformation("Started background service to remove expired email verification tokens at: {dateTime}", DateTime.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                int deletedCount = await context.Set<EmailVerificationToken>()
                    .Where(t => t.ExpiresAt <= DateTime.Now)
                    .ExecuteDeleteAsync(stoppingToken);

                _logger.LogInformation("Deleted {count} expired email verification tokens at: {dateTime}", deletedCount, DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting expired email verification tokens.");
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
