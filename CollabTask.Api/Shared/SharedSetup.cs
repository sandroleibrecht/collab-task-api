using CollabTask.Api.Shared.Helpers;

namespace CollabTask.Api.Shared
{
	public static class SharedSetup
	{
		public static IServiceCollection AddShared(this IServiceCollection services)
		{
			services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();

			return services;
		}
	}
}
