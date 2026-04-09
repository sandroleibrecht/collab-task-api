using CollabTask.Shared.DTOs.Auth;

namespace CollabTask.Shared.Models
{
	public class LoginResult
	{
		public bool IsSuccess { get; set; }
		public AuthResponseDto? UserData { get; set; }
		public string? ErrorMessage { get; set; }
		public List<ValidationFailure>? ValidationErrors { get; set; }
	}
}
