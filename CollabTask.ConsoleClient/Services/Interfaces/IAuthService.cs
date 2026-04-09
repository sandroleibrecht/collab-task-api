using CollabTask.ConsoleClient.DTOs;
using CollabTask.Shared.DTOs;

namespace CollabTask.ConsoleClient.Services.Interfaces;

public interface IAuthService
{
	Task<LoginResult> LoginAsync(SignInDto signInDto);
	bool TryToGetAdminCredentials(out SignInDto signInDto);
}
