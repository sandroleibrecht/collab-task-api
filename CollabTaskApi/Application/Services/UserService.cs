using CollabTaskApi.Application.Interfaces;
using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.DTOs.User;
using CollabTaskApi.Domain.Models;
using CollabTaskApi.Infrastructure.Data;
using CollabTaskApi.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Application.Services
{
	public class UserService(AppDbContext context, IPasswordHasher hasher) : IUserService
	{
		private readonly AppDbContext _context = context;
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

			var currentImage = await _context.UserImages.SingleOrDefaultAsync(i => i.UserId == user.Id);
			var newImage = new UserImage();

			if (dto.Image is not null)
			{
				if (currentImage is not null)
				{
					currentImage.FilePath = dto.Image;
				}
				else
				{
					newImage.UserId = userId;
					newImage.FilePath = dto.Image;
					await _context.UserImages.AddAsync(newImage);
				}
			}

			await _context.SaveChangesAsync();

			return new BoardUserDto
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				ImagePath = dto.Image is null ? currentImage?.FilePath : dto.Image,
			};
		}

		public async Task DeleteAsync(int userId)
		{
			var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId)
			?? throw new ArgumentException("Unable to find user with the provided Id");

			// Remove:
			// * UserImage
			// * UserDesk (random anderer member wird admin)
			// * Desk (nur die wo sonst keine member)
			// * DeskInvitation (wo userId receiver oder sender)
			// * UserRefreshToken
			// * User

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
		}
	}
}
