using CollabTaskApi.Helpers.Auth;
using CollabTaskApi.Helpers.Auth.Interfaces;

namespace CollabTaskApi.Helpers
{
	public static class HelperRegistration
	{
		public static IServiceCollection AddHelpers(this IServiceCollection services)
		{
			services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();

			return services;
		}
	}
}
