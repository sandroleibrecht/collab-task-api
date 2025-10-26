using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Data
{
	public static class DbRegistration
	{
		public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<AppDbContext>(
				options =>
				options.UseSqlite(config.GetConnectionString("DefaultConnection")));

			return services;
		}
	}
}
