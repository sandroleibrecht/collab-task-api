using CollabTaskApi.Domain.Models;

namespace CollabTaskApi.Application.Interfaces
{
	public interface IImageService
	{
		Task<UserImage?> GetUserImageAsync(int userId);
		Task DeleteUserImageAsync(int userId);
		Task<string?> UpdateUserImageAsync(int userId, string? filePath);
	}
}