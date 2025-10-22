using CollabTaskApi.DTOs;
using CollabTaskApi.Models;

namespace CollabTaskApi.Mappers
{
	public static class UserMapper
	{
		public static UserDto ToDto(User u)
		{
			return new UserDto
			{
				Id = u.Id,
				Name = u.Name,
				Email = u.Email
			};
		}

		public static User ToUser(UserCreateDto u)
		{
			return new User
			{
				Name = u.Name,
				Email = u.Email,
				Password = u.Password
			};
		}

		public static User Update(User existing, UserUpdateDto update)
		{
			if (update.Name != null) existing.Name = update.Name;
			if (update.Email != null) existing.Email = update.Email;
			if (update.Password != null) existing.Password = update.Password;

			return existing;
		}
	}
}
