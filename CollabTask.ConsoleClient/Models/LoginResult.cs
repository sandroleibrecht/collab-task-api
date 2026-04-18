using CollabTask.Shared.DTOs.Auth;

namespace CollabTask.ConsoleClient.Models
{
	public class LoginResult
	{
		public bool IsSuccess { get; set; }
		public AuthResponseDto? UserData { get; set; }
		public string? ErrorMessage { get; set; }
		public List<ValidationError>? ValidationErrors { get; set; }
	}
}
