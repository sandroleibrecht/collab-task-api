using System.Net.Http.Json;
using System.Text.Json;
using Serilog;
using CollabTask.Shared.DTOs.Auth;
using CollabTask.ConsoleClient.Services.Interfaces;
using CollabTask.ConsoleClient.UI.Interfaces;
using CollabTask.ConsoleClient.Models;

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

		bool loginAsAdmin = false;

		if (_authService.TryToBuildAdminSignInDto(out SignInDto signInDto))
		{
			Console.WriteLine("Admin credentials detected. Do you want to login as admin? [Y]es/[N]o");
			loginAsAdmin = Console.ReadLine()?.ToLower() == "y";
		}

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

			LoginResult loginResult = await _authService.SignInAsync(signInDto);

			if (!loginResult.IsSuccess)
			{
				if (!string.IsNullOrEmpty(loginResult.ErrorMessage))
				{
					Console.WriteLine(loginResult.ErrorMessage);
				}

				if (loginResult.ValidationErrors != null && loginResult.ValidationErrors.Count != 0)
				{
					foreach (var err in loginResult.ValidationErrors)
					{
						Console.WriteLine($"{err.ErrorMessage}");
					}
				}

				Console.WriteLine("Login failed. [L]ogin, [S]ign up");

				if (Console.ReadLine() is "L" or (not "L" and not "S"))
				{
					loginAsAdmin = false;
					continue;
				}

				while (true)
				{
					Console.WriteLine("== Sign up ==");

					Console.Write("Username: ");
					var nameInput = Console.ReadLine();

					Console.Write("Email: ");
					var emailInput = Console.ReadLine();
					
					Console.Write("Password: ");
					var passInput = Console.ReadLine();

					if (string.IsNullOrEmpty(nameInput) || string.IsNullOrEmpty(emailInput) || string.IsNullOrEmpty(passInput))
					{
						continue;
					}

					SignUpDto signUpDto = new()
					{
						Name = nameInput,
						Email = emailInput,
						Password = passInput
					};

					loginResult = await _authService.SignUpAsync(signUpDto);

					if (!loginResult.IsSuccess)
					{
						if (!string.IsNullOrEmpty(loginResult.ErrorMessage))
						{
							Console.WriteLine(loginResult.ErrorMessage);
						}

						if (loginResult.ValidationErrors != null && loginResult.ValidationErrors.Count != 0)
						{
							foreach (var err in loginResult.ValidationErrors)
							{
								Console.WriteLine($"{err.ErrorMessage}");
							}
						}

						continue;
					}
					
					break;
				}
			}

			var resContent = loginResult.UserData ?? throw new Exception("Unexpected authentication error");

			Console.WriteLine("Login success");

			_appState.UserName = resContent.User.Name;
			_appState.UserEmail = resContent.User.Email;
			_appState.UserAccessToken = resContent.AccessToken;
			_appState.UserRefreshToken = resContent.RefreshToken;

			_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _appState.UserAccessToken);

			break;
		}
	}
}
