using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.DTOs.Board;
using CollabTaskApi.DTOs.User;

namespace CollabTaskApi.Services.Interfaces
{
	public interface IUserService
	{
		Task<BoardUserDto?> GetBoardUserDtoAsync(int userId);
		Task<UserDto> CreateAsync(SignUpDto user);
		//Task<IEnumerable<User>> GetAll();
		//Task<User?> GetById(int id);
		//Task<User> Update(User user);
		//Task<bool> Delete(int id);
	}
}