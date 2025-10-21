using AutoMapper;
using CollabTaskApi.Models;
using CollabTaskApi.DTOs;

namespace CollabTaskApi.Mappings
{
	public class AppMappingProfile : Profile
	{
		public AppMappingProfile()
		{
			CreateMap<User, UserDto>();
			CreateMap<UserCreateDto, User>();
		}
	}
}
