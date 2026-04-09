using CollabTask.Shared.DTOs.User;

namespace CollabTask.Shared.DTOs.Auth
{
	public class AuthResponseDto
	{
		public string AccessToken { get; set; } = string.Empty;
		public string RefreshToken { get; set; } = null!;
		public UserDto User { get; set; } = new();
	}
}
