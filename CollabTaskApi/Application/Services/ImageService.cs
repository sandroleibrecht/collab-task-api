using CollabTaskApi.Application.Interfaces;
using CollabTaskApi.Domain.Models;
using CollabTaskApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Application.Services
{
	public class ImageService(AppDbContext context) : IImageService
	{
		private readonly AppDbContext _context = context;

		public async Task<UserImage?> GetUserImageAsync(int userId)
		{
			return await _context.UserImages.SingleOrDefaultAsync(i => i.UserId == userId);
		}
		public async Task DeleteUserImageAsync(int userId)
		{
			var currentImage = await GetUserImageAsync(userId);
			if (currentImage is null) return;

			_context.UserImages.Remove(currentImage);
			await _context.SaveChangesAsync();
		}

		public async Task<string?> UpdateUserImageAsync(int userId, string? filePath)
		{
			var currentImage = await GetUserImageAsync(userId);

			if (string.IsNullOrEmpty(filePath) && currentImage is null)
				return null;

			if (string.IsNullOrEmpty(filePath) && currentImage is not null)
				return currentImage.FilePath;

			if (!string.IsNullOrEmpty(filePath))
			{
				if (currentImage is not null)
				{
					currentImage.FilePath = filePath;
				}
				else
				{
					await _context.UserImages.AddAsync(new UserImage
					{
						UserId = userId,
						FilePath = filePath
					});
				}
				await _context.SaveChangesAsync();
			}

			return filePath;
		}
	}
}
