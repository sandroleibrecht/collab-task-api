using CollabTask.Shared.DTOs.Auth;
using CollabTask.Api.Domain.Models;

namespace CollabTask.Api.Application.Interfaces
{
	public interface IAuthService
	{
		Task<AuthResponseDto?> SignUpAsync(SignUpDto dto);
		Task<AuthResponseDto?> SignInAsync(SignInDto dto);
		Task<AuthResponseDto> BuildAuthResponse(User user);
	}
}
