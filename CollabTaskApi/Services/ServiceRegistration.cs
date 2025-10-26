using CollabTaskApi.Services.Application;
using CollabTaskApi.Services.Application.Interfaces;
using CollabTaskApi.Services.Background;

namespace CollabTaskApi.Services
{
	public static class ServiceRegistration
	{
		public static IServiceCollection AddServices(this IServiceCollection services)
		{
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IDeskService, DeskService>();
			services.AddScoped<IBoardService, BoardService>();
			services.AddScoped<IInviteService, InviteService>();
			services.AddScoped<IJwtService, JwtService>();

			services.AddHostedService<TokenCleanupService>();

			return services;
		}
	}
}
