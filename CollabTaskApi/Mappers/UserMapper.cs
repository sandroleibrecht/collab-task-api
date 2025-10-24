using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.DTOs.User;
using CollabTaskApi.Models;
using CollabTaskApi.Mappers.Interfaces;

namespace CollabTaskApi.Mappers
{
	public class UserMapper : IUserMapper
	{
		public User Map(SignUpDto src)
		{
			return new User
			{
				Name = src.Name,
				Email = src.Email,
				Password = src.Password,
				CreatedAt = DateTime.UtcNow
			};
		}

		public UserDto Map(User src)
		{
			return new UserDto
			{
				Id = src.Id,
				Name = src.Name,
				Email = src.Email
			};
		}
	}
}
