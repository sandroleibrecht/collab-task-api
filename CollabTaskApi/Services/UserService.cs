using CollabTaskApi.Data;
using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.DTOs.Board;
using CollabTaskApi.DTOs.User;
using CollabTaskApi.Models;
using CollabTaskApi.Mappers.Interfaces;
using CollabTaskApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Services
{
	public class UserService(AppDbContext context, IUserMapper userMapper) : IUserService
	{
		private readonly AppDbContext _context = context;
		private readonly IUserMapper _userMapper = userMapper;

		public async Task<BoardUserDto?> GetBoardUserDtoAsync(int userId)
		{
			var dto = await (
				from u in _context.Users
				join iGrp in _context.UserImages on u.Id equals iGrp.UserId into userImages
				from i in userImages.DefaultIfEmpty()
				where u.Id == userId
				select new BoardUserDto
				{
					Id = u.Id,
					Name = u.Name,
					Email = u.Email,
					ImagePath = i.FilePath
				}
			)
			.AsNoTracking()
			.FirstOrDefaultAsync();

			return dto;
		}

		public async Task<UserDto> CreateAsync(SignUpDto dto)
		{
			User user = _userMapper.Map(dto);

			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();

			UserDto userDto = _userMapper.Map(user);

			return userDto;
		}

		public async Task<AuthResponseDto?> SignInAsync(SignInDto dto)
		{
			var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);

			if (user is null) return null;

			if (user.Password != dto.Password) return null;


			// --- password system tbd
			// --- jwt system tbd


			UserDto userDto = _userMapper.Map(user);

			return new AuthResponseDto
			{
				AccessToken = "TestAccessToken",
				RefreshToken = "TestRefreshToken",
				User = userDto
			};
		}
	}
}
