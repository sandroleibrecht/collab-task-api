using CollabTask.Api.Domain.DTOs.Board;
using CollabTask.Api.Domain.DTOs.User;
using CollabTask.Api.Domain.Models;

namespace CollabTask.Api.Application.Interfaces
{
	public interface IUserService
	{
		Task<User?> GetUserByIdAsync(int id);
		Task<User?> GetUserByEmailAsync(string email);
		Task<BoardUserDto?> GetBoardUserDtoAsync(int userId);
		Task<bool> CreateAsync(User user);
		Task<BoardUserDto?> UpdateAsync(int userId, UserUpdateDto dto);
		Task DeleteAsync(int userId);
	}
}