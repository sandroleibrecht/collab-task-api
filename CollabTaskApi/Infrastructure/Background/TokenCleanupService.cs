using Microsoft.EntityFrameworkCore;
using CollabTaskApi.Infrastructure.Data;

namespace CollabTaskApi.Infrastructure.Background
{
	public class TokenCleanupService(
		IServiceScopeFactory scopeFactory,
		ILogger<TokenCleanupService> logger,
		IWebHostEnvironment env) : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
		private readonly ILogger<TokenCleanupService> _logger = logger;
		private readonly TimeSpan _interval = env.IsDevelopment() ? TimeSpan.FromMinutes(30) : TimeSpan.FromHours(24);
		private readonly int _retryMinutes = 5;
		private readonly Random _random = new();

		protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			var startupDelay = TimeSpan.FromMinutes(_random.Next(0, 5));
			
			_logger.LogInformation("TokenCleanupService: Starting after {Delay} minutes", startupDelay.TotalMinutes);
			
			await Task.Delay(startupDelay, cancellationToken);

			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					await CleanupExpiredTokensAsync(cancellationToken);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "TokenCleanupService: Error during cleanup, retrying in {RetryMinutes} minutes", _retryMinutes);
					await Task.Delay(TimeSpan.FromMinutes(_retryMinutes), cancellationToken);
				}

				_logger.LogInformation("TokenCleanupService: Next run in {Hours} hours", _interval.TotalHours);
				await Task.Delay(_interval, cancellationToken);
			}
		}

		private async Task CleanupExpiredTokensAsync(CancellationToken ct)
		{
			using var scope = _scopeFactory.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			var expiredTokens = await context.UserRefreshToken
				.Where(t => t.ExpiresAt < DateTime.UtcNow)
				.ToListAsync(ct);

			if (expiredTokens.Count != 0)
			{
				context.UserRefreshToken.RemoveRange(expiredTokens);
				await context.SaveChangesAsync(ct);
				
				_logger.LogInformation("TokenCleanupService: Removed {Count} expired refresh tokens", expiredTokens.Count);
			}
			else
			{
				_logger.LogInformation("TokenCleanupService: No expired tokens found");
			}
		}
	}
}
