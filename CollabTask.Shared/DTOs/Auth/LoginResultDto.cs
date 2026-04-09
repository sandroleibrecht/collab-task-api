using CollabTask.Shared.Models;

namespace CollabTask.Shared.DTOs.Auth
{
	public class LoginResultDto
	{
		public bool IsSuccess { get; set; }
		public AuthResponseDto? UserData { get; set; }
		public string? ErrorMessage { get; set; }
		public List<ValidationFailure>? ValidationErrors { get; set; }
	}
}
