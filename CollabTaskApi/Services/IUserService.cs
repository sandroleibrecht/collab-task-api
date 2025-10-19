using CollabTaskApi.DTOs;
using CollabTaskApi.Models;

namespace CollabTaskApi.Services
{
	public interface IUserService
	{
		Task<IEnumerable<User>> GetAll();
		Task<User> Create(User user);
		Task<bool> Delete(int id);
	}
}