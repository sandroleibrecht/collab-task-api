using CollabTask.Api.Application.Interfaces;
using CollabTask.Api.Application.Services;

namespace CollabTask.Api.Application
{
	public static class ApplicationSetup
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IDeskService, DeskService>();
			services.AddScoped<IBoardService, BoardService>();
			services.AddScoped<IInviteService, InviteService>();
			services.AddScoped<IImageService, ImageService>();
			services.AddScoped<IJwtService, JwtService>();

			return services;
		}
	}
}
