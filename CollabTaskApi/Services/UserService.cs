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

		//public async Task<IEnumerable<User>> GetAll()
		//{
		//	return await _context.Users.AsNoTracking().ToListAsync();
		//}

		//public async Task<User?> GetById(int id)
		//{
		//	return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
		//}

		//public async Task<User> Update(User user)
		//{
		//	_context.Users.Update(user);
		//	await _context.SaveChangesAsync();

		//	return user;
		//}

		//public async Task<bool> Delete(int id)
		//{
		//	var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

		//	if (user == null) return false;

		//	_context.Users.Remove(user);

		//	await _context.SaveChangesAsync();

		//	return true;
		//}
	}
}
