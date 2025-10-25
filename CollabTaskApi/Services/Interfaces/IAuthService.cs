using CollabTaskApi.DTOs.Auth;

namespace CollabTaskApi.Services.Interfaces
{
	public interface IAuthService
	{
		Task<AuthResponseDto?> SignUpAsync(SignUpDto dto);
		Task<AuthResponseDto?> SignInAsync(SignInDto dto);
	}
}
