using CollabTaskApi.Application.Interfaces;
using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.DTOs.User;
using CollabTaskApi.Domain.Models;
using CollabTaskApi.Infrastructure.Data;
using CollabTaskApi.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Application.Services
{
	public class UserService(
		AppDbContext context,
		IImageService imageService,
		IDeskService deskService,
		IJwtService jwtService,
		IInviteService inviteService,
		IPasswordHasher hasher) : IUserService
	{
		private readonly AppDbContext _context = context;
		private readonly IImageService _imageService = imageService;
		private readonly IDeskService _deskService = deskService;
		private readonly IJwtService _jwtService = jwtService;
		private readonly IInviteService _inviteService = inviteService;
		private readonly IPasswordHasher _hasher = hasher;

		public async Task<User?> GetUserByIdAsync(int id) => await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

		public async Task<User?> GetUserByEmailAsync(string email) => await _context.Users.SingleOrDefaultAsync(u => u.Email == email.Trim());

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

		public async Task<BoardUserDto?> UpdateAsync(int userId, UserUpdateDto dto)
		{
			var user = await GetUserByIdAsync(userId);
			if (user is null) return null;

			user.Name = dto.Name ?? user.Name;
			user.Email = dto.Email ?? user.Email;
			user.Password = dto.Password is not null ? _hasher.Hash(dto.Password) : user.Password;
			
			await _context.SaveChangesAsync();

			var image = await _imageService.UpdateUserImageAsync(user.Id, dto.Image);

			return new BoardUserDto
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				ImagePath = image
			};
		}

		public async Task DeleteAsync(int userId)
		{
			var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId)
			?? throw new ArgumentException("Unable to find user with the provided Id");

			await _imageService.DeleteUserImageAsync(user.Id);
			await _jwtService.RemoveRefreshTokenAsync(user.Id);
			await _inviteService.DeleteAllInvitationsByUserIdAsync(user.Id);

			var desks = await _deskService.GetAllDesksAsync(userId);

			foreach (var desk in desks)
			{
				await _deskService.HandleUserLeaveAsync(userId, desk);
			}

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
		}
	}
}
