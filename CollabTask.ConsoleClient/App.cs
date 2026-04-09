using CollabTask.ConsoleClient.Services.Interfaces;
using CollabTask.ConsoleClient.UI.Views;
using Serilog;

namespace CollabTask.ConsoleClient;

public class App(
	AuthView authView,
	BoardView boardView,
	IViewService viewService,
	ILogger logger)
{
	private readonly AuthView _authView = authView;
	private readonly BoardView _boardView = boardView;
	private readonly IViewService _viewService = viewService;
	private readonly ILogger _logger = logger.ForContext<App>();

	public async Task RunAsync()
	{
		// start auth
		await _viewService.ShowView(_authView);

		// access main interface loop after auth
		while (true)
		{
			Console.Write("[B]oard, [E]xit >> ");

			switch (Console.ReadLine())
			{
				case "B": 
					await _viewService.ShowView(_boardView);
					break;
				case "E":
					ShutdownApplication();
					break;
				default:
					Console.WriteLine("Invalid command");
					break;
			}
		}
	}

	private void ShutdownApplication()
	{
		_logger.Information("Application shutdown");
		Log.CloseAndFlush();
		Environment.Exit(0);
	}
}
