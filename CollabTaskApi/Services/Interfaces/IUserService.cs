using CollabTaskApi.DTOs.Board;
using CollabTaskApi.Models;

namespace CollabTaskApi.Services.Interfaces
{
	public interface IUserService
	{
		Task<User?> GetUserByEmailAsync(string email);
		Task<BoardUserDto?> GetBoardUserDtoAsync(int userId);
		Task<bool> CreateAsync(User user);
		Task<bool> DeleteAsync(int userId);
	}
}