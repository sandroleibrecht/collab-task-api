using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.DTOs.User;
using CollabTaskApi.Models;

namespace CollabTaskApi.Mappers.Interfaces
{
	public interface IUserMapper
	{
		User Map(SignUpDto source);
		UserDto Map(User source);
	}
}
