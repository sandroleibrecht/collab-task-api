using CollabTask.ConsoleClient;
using CollabTask.ConsoleClient.Services;
using CollabTask.ConsoleClient.Services.Interfaces;
using CollabTask.ConsoleClient.UI.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

internal class Program
{
	public static async Task Main()
	{
		// build config
		IConfiguration config = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddUserSecrets<Program>()
			.Build();

		// create app state
		var appState = new AppState();

		// http client config
		var httpClient = new HttpClient()
		{
			BaseAddress = new Uri(config["Api:Url"] ?? throw new KeyNotFoundException("Configuration error: API URL not provided"))
		};

		// serilog config
		Log.Logger = new LoggerConfiguration()
			.WriteTo.File(
				path: Path.Combine(Directory.GetCurrentDirectory(), "logs/log-.txt"),
				rollingInterval: RollingInterval.Day,
				retainedFileCountLimit: 7,
				outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
			)
			.CreateLogger();
		
		// register services
		var services = new ServiceCollection();
		services.AddSingleton(config);
		services.AddSingleton(appState);
		services.AddSingleton(httpClient);
		services.AddSingleton(Log.Logger);
		services.AddTransient<App>();
		services.AddTransient<AuthView>();
		services.AddTransient<BoardView>();
		services.AddTransient<IViewService, ViewService>();
		services.AddTransient<IAuthService, AuthService>();

		// build and run
		var serviceProvider = services.BuildServiceProvider();
		var app = serviceProvider.GetRequiredService<App>();
		await app.RunAsync();
	}
}