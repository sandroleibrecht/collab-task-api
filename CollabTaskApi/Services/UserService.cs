using CollabTaskApi.Data;
using CollabTaskApi.DTOs.Board;
using CollabTaskApi.Models;
using CollabTaskApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Services
{
	public class UserService(AppDbContext context) : IUserService
	{
		private readonly AppDbContext _context = context;

		public async Task<User?> GetUserByEmailAsync(string email)
		{
			return await _context.Users.SingleOrDefaultAsync(u => u.Email == email.Trim());
		}

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

		public async Task<bool> CreateAsync(User user)
		{
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteAsync(int userId)
		{
			var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
			if (user is null) return false;
			
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
			
			return true;
		}
	}
}
