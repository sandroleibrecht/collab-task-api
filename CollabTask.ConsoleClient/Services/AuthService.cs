using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Json;
using CollabTask.ConsoleClient.Services.Interfaces;
using CollabTask.Shared.DTOs.Auth;
using CollabTask.Shared.Models;

namespace CollabTask.ConsoleClient.Services;

public class AuthService(IConfiguration config, HttpClient httpClient) : IAuthService
{
	private readonly IConfiguration _config = config;
	private readonly HttpClient _httpClient = httpClient;

	public async Task<LoginResultDto> LoginAsync(SignInDto signInDto)
	{
		var res = await _httpClient.PostAsJsonAsync("/api/auth/signin", signInDto);

		if (res.IsSuccessStatusCode)
		{
			var userData = await res.Content.ReadFromJsonAsync<AuthResponseDto>();
			return new LoginResultDto() { IsSuccess = true, UserData = userData };
		}

		if (res.StatusCode == HttpStatusCode.BadRequest)
		{
			var validationErrors = await res.Content.ReadFromJsonAsync<List<ValidationFailure>>();
			return new LoginResultDto() { IsSuccess = false, ErrorMessage = "Validation errors have occurred.", ValidationErrors = validationErrors };
		}

		if (res.StatusCode == HttpStatusCode.Unauthorized)
		{
			return new LoginResultDto() { IsSuccess = false, ErrorMessage = "Credentials are invalid." };
		}

		return new LoginResultDto() { IsSuccess = false, ErrorMessage = "An unexpected error has occurred."};
	}

	public bool TryToGetAdminCredentials(out SignInDto signInDto)
	{
		var adminMail = _config["AdminCreds:Email"];
		var adminPass = _config["AdminCreds:Password"];

		if (adminMail is not null && adminPass is not null)
		{
			signInDto = new()
			{
				Email = adminMail,
				Password = adminPass
			};

			return true;
		}

		signInDto = new();
		
		return false;
	}
}
