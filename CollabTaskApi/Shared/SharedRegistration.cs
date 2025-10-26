using CollabTaskApi.Shared.Helpers;

namespace CollabTaskApi.Shared
{
	public static class SharedRegistration
	{
		public static IServiceCollection AddShared(this IServiceCollection services)
		{
			services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();

			return services;
		}
	}
}
