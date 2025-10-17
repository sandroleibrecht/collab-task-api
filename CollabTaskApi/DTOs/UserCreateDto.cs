using System.ComponentModel.DataAnnotations;

namespace CollabTaskApi.DTOs
{
	public class UserCreateDto
	{
		[Required(ErrorMessage = "Name is required")]
		[MinLength(2, ErrorMessage = "Name must be at least 2 characters long")]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; } = string.Empty;
	}
}
