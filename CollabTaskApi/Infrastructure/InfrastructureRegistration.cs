using CollabTaskApi.Infrastructure.Data;
using CollabTaskApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Infrastructure
{
	public static class InfrastructureRegistration
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<AppDbContext>(
				options =>
				options.UseSqlite(config.GetConnectionString("DefaultConnection")));

			services.AddHostedService<TokenCleanupService>();
			
			return services;
		}
	}
}
