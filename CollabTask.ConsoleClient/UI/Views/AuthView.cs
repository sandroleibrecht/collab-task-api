using System.Net.Http.Json;
using System.Text.Json;
using Serilog;
using CollabTask.Shared.DTOs.Auth;
using CollabTask.ConsoleClient.DTOs;
using CollabTask.ConsoleClient.Services.Interfaces;
using CollabTask.ConsoleClient.UI.Interfaces;

namespace CollabTask.ConsoleClient.UI.Views;

public class AuthView(AppState appState, HttpClient client, ILogger logger, IAuthService authService) : IView
{
	private readonly AppState _appState = appState;
	private readonly HttpClient _client = client;
	private readonly ILogger _logger = logger.ForContext<AuthView>();
	private readonly IAuthService _authService = authService;

	public async Task Show()
	{
		Console.WriteLine("== Login ==");

		SignInDto signInDto = new();
		bool loginAsAdmin = false;

		if (_authService.TryToGetAdminCredentials(out signInDto))
		{
			Console.WriteLine("Admin credentials detected. Do you want to login as admin? [Y]es/[N]o");
			loginAsAdmin = Console.ReadLine()?.ToLower() == "y";
		}

		//var loginResult = await _authService.LoginAsync(signInDto);

		while (true)
		{
			if (!loginAsAdmin)
			{
                Console.Write("Enter email: ");
				var emailInput = Console.ReadLine();

				Console.Write("Enter password: ");
				var passInput = Console.ReadLine();

				if (string.IsNullOrEmpty(emailInput) || string.IsNullOrEmpty(passInput))
				{
					continue;
				}
				else
				{
					signInDto.Email = emailInput;
					signInDto.Password = passInput;
				}
			}

			var authRes = await _client.PostAsJsonAsync("/api/auth/signin", signInDto);
			
			AuthResponseDto? content;

			if (!authRes.IsSuccessStatusCode)
			{
				Console.WriteLine("Login failed. [L]ogin, [S]ign up");

				var action = Console.ReadLine();

				if (action is "L" or (not "L" and not "S"))
				{
					loginAsAdmin = false;
					continue;
				}

				while (true)
				{
					Console.WriteLine("== Sign up ==");
					Console.Write("Username: ");
					var userName = Console.ReadLine();
					Console.Write("Email: ");
					var userMail = Console.ReadLine();
					Console.Write("Password: ");
					var userPass = Console.ReadLine();

					var adminSignUp = new { Name = userName, Email = userMail, Password =  userPass};
					var signUpRes = await _client.PostAsJsonAsync("/api/auth/signup", adminSignUp);

					if (!signUpRes.IsSuccessStatusCode)
					{
						var errorContent = await signUpRes.Content.ReadAsStringAsync();
						var errorJson = JsonDocument.Parse(errorContent);

						foreach (var error in errorJson.RootElement.EnumerateArray())
						{
							var errorMessage = error.GetProperty("errorMessage").GetString();

							Console.WriteLine($"{errorMessage}");
						}

						continue;
					}

					content = await signUpRes.Content.ReadFromJsonAsync<AuthResponseDto>();
					break;
				}
			}
			else
			{
				Console.WriteLine("Login success");
				content = await authRes.Content.ReadFromJsonAsync<AuthResponseDto>();
			}

			if (content is null) throw new Exception("Authentication error");

			_appState.UserName = content.User.Name;
			_appState.UserEmail = content.User.Email;
			_appState.UserAccessToken = content.AccessToken;
			_appState.UserRefreshToken = content.RefreshToken;

			_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _appState.UserAccessToken);

			break;
		}
	}
}
