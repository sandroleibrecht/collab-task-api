using CollabTaskApi.Domain.DTOs.Auth;
using CollabTaskApi.Domain.Models;

namespace CollabTaskApi.Application.Interfaces
{
	public interface IAuthService
	{
		Task<AuthResponseDto?> SignUpAsync(SignUpDto dto);
		Task<AuthResponseDto?> SignInAsync(SignInDto dto);
		Task<AuthResponseDto> BuildAuthResponse(User user);
	}
}
