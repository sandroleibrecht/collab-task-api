using CollabTask.Shared.DTOs.Board;
using CollabTask.ConsoleClient.UI.Interfaces;
using System.Net.Http.Json;

namespace CollabTask.ConsoleClient.UI.Views;

public class BoardView(AppState appState, HttpClient client) : IView
{
	private readonly AppState _appState = appState;
	private readonly HttpClient _client = client;

	public async Task Show()
	{
		while (true)
		{
			Console.WriteLine("== Board ==");

			var boardRes = await _client.GetAsync("/api/board");

			if (!boardRes.IsSuccessStatusCode) throw new Exception("Unable to get Board...");

			var boardContent = await boardRes.Content.ReadFromJsonAsync<BoardDto>();

			if (boardContent is null) throw new Exception("Board content must not be null");

			List<BoardDeskDto> allDesks = [.. boardContent.OwnedDesks, .. boardContent.SharedDesks];
			int totalDeskCount = allDesks.Count;

			if (totalDeskCount == 0)
			{
				Console.WriteLine("No desks");
			}
			else
			{
				Console.WriteLine("Desks:");
			
				int count = 1;
			
				foreach(var desk in allDesks)
				{
					string prefix = $"{count++} ({desk.UserDeskType})".PadRight(15);
					Console.WriteLine($"{prefix} {desk.Name}");
				}
			}

			Console.WriteLine("Enter number of desk, [C]reate desk, [I]nvitations, [E]dit user, [B]ack");

			var action = Console.ReadLine();

			if (int.TryParse(action, out var deskNum) && deskNum > 0 && deskNum <= totalDeskCount)
			{
				Console.WriteLine("Open desk...");
			}
			else if (action == "C")
			{
				Console.WriteLine("== Create Desk ==");
			}
			else if (action == "I")
			{
				Console.WriteLine("== Invitations ==");
			}
			else if (action == "E")
			{
				Console.WriteLine("== Edit User ==");
			}
			else if (action == "B")
			{
				return;
			}
		}
	}
}
