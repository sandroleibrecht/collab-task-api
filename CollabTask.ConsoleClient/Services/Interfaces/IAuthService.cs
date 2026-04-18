using CollabTask.ConsoleClient.Models;
using CollabTask.Shared.DTOs.Auth;

namespace CollabTask.ConsoleClient.Services.Interfaces;

public interface IAuthService
{
	Task<LoginResult> SignInAsync(SignInDto signInDto);
	Task<LoginResult> SignUpAsync(SignUpDto signUpDto);
	bool TryToBuildAdminSignInDto(out SignInDto signInDto);
}
