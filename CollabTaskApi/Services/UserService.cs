using CollabTaskApi.DTOs;
using CollabTaskApi.Models;

namespace CollabTaskApi.Services
{
	public class UserService
	{
		private static readonly List<User> _users = new();

		public IEnumerable<User> GetAll() => _users;

		public User Create(UserCreateDto dto)
		{
			var user = new User
			{
				Id = _users.Count + 1,
				Name = dto.Name,
				Email = dto.Email
			};

			_users.Add(user);
			return user;
		}
	}
}
