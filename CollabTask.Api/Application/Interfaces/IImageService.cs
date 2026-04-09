using CollabTask.Api.Domain.Models;

namespace CollabTask.Api.Application.Interfaces
{
	public interface IImageService
	{
		Task<UserImage?> GetUserImageAsync(int userId);
		Task DeleteUserImageAsync(int userId);
		Task<string?> UpdateUserImageAsync(int userId, string? filePath);
	}
}