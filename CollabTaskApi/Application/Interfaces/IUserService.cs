using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.DTOs.User;
using CollabTaskApi.Domain.Models;

namespace CollabTaskApi.Application.Interfaces
{
	public interface IUserService
	{
		Task<User?> GetUserByIdAsync(int id);
		Task<User?> GetUserByEmailAsync(string email);
		Task<BoardUserDto?> GetBoardUserDtoAsync(int userId);
		Task<bool> CreateAsync(User user);
		Task<BoardUserDto?> UpdateAsync(int userId, UserUpdateDto dto);
		Task<bool> DeleteAsync(int userId);
	}
}