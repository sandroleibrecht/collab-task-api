namespace CollabTaskApi.Options
{
	public static class OptionRegistration
	{
		public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration config)
		{
			services.Configure<JwtOptions>(config.GetSection("Jwt"));

			return services;
		}
	}
}
