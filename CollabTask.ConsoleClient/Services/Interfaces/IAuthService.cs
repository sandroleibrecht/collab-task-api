using CollabTask.Shared.DTOs.Auth;
using CollabTask.Shared.Models;

namespace CollabTask.ConsoleClient.Services.Interfaces;

public interface IAuthService
{
	Task<LoginResult> LoginAsync(SignInDto signInDto);
	bool TryToGetAdminCredentials(out SignInDto signInDto);
}
