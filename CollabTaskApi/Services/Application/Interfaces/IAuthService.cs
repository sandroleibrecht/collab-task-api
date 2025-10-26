using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.Models;

namespace CollabTaskApi.Services.Application.Interfaces
{
	public interface IAuthService
	{
		Task<AuthResponseDto?> SignUpAsync(SignUpDto dto);
		Task<AuthResponseDto?> SignInAsync(SignInDto dto);
		Task<AuthResponseDto> BuildAuthResponse(User user);
	}
}
