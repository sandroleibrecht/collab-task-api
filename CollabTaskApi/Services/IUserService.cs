using CollabTaskApi.Models;

namespace CollabTaskApi.Services
{
	public interface IUserService
	{
		Task<IEnumerable<User>> GetAll();
		Task<User?> GetById(int id);
		Task<User> Create(User user);
		Task<User> Update(User user);
		Task<bool> Delete(int id);
	}
}