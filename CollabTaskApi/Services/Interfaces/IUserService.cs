using CollabTaskApi.DTOs.Board;
using CollabTaskApi.Models;

namespace CollabTaskApi.Services.Interfaces
{
	public interface IUserService
	{
		Task<IEnumerable<User>> GetAll();
		Task<User?> GetById(int id);
		Task<BoardUserDto?> GetBoardUserDto(int userId);
		Task<User> Create(User user);
		Task<User> Update(User user);
		Task<bool> Delete(int id);
	}
}