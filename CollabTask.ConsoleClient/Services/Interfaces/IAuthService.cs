using CollabTask.Shared.DTOs.Auth;

namespace CollabTask.ConsoleClient.Services.Interfaces;

public interface IAuthService
{
	Task<LoginResultDto> LoginAsync(SignInDto signInDto);
	bool TryToGetAdminCredentials(out SignInDto signInDto);
}
