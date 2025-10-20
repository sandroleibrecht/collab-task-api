using System.ComponentModel.DataAnnotations;

namespace CollabTaskApi.DTOs
{
	public class UserUpdateDto
	{
		[MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
		public string? Name { get; set; }

		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string? Email { get; set; }
	}
}
