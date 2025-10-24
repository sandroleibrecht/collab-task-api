using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.DTOs.Board;
using CollabTaskApi.DTOs.User;

namespace CollabTaskApi.Services.Interfaces
{
	public interface IUserService
	{
		Task<BoardUserDto?> GetBoardUserDtoAsync(int userId);
		Task<UserDto> CreateAsync(SignUpDto dto);
		Task<AuthResponseDto?> SignInAsync(SignInDto dto);
	}
}