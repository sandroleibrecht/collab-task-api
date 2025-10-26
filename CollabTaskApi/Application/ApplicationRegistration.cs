using CollabTaskApi.Application.Interfaces;
using CollabTaskApi.Application.Services;

namespace CollabTaskApi.Application
{
	public static class ApplicationRegistration
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IDeskService, DeskService>();
			services.AddScoped<IBoardService, BoardService>();
			services.AddScoped<IInviteService, InviteService>();
			services.AddScoped<IJwtService, JwtService>();

			return services;
		}
	}
}
