using CollabTask.Api.Domain.DTOs.User;

namespace CollabTask.Api.Domain.DTOs.Auth
{
	public class AuthResponseDto
	{
		public string AccessToken { get; set; } = string.Empty;
		public string RefreshToken { get; set; } = null!;
		public UserDto User { get; set; } = new();
	}
}
