using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.Models;

namespace CollabTaskApi.Application.Interfaces
{
	public interface IUserService
	{
		Task<User?> GetUserByEmailAsync(string email);
		Task<BoardUserDto?> GetBoardUserDtoAsync(int userId);
		Task<bool> CreateAsync(User user);
		Task<bool> DeleteAsync(int userId);
	}
}