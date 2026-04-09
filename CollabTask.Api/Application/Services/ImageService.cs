using CollabTask.Api.Application.Interfaces;
using CollabTask.Api.Domain.Models;
using CollabTask.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CollabTask.Api.Application.Services
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

			if (string.IsNullOrEmpty(filePath))
			{
				return currentImage?.FilePath;
			}

			if (currentImage is not null)
			{
				currentImage.FilePath = filePath;
			}
			else
			{
				UserImage img = new()
				{
					UserId = userId,
					FilePath = filePath
				};

				await _context.UserImages.AddAsync(img);
			}

			await _context.SaveChangesAsync();

			return filePath;
		}
	}
}
