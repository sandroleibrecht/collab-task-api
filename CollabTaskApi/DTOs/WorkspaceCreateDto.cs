using System.ComponentModel.DataAnnotations;

namespace CollabTaskApi.DTOs
{
	public class WorkspaceCreateDto
	{
		[Required(ErrorMessage = "Name is required.")]
		[MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
		public string Name { get; set; } = string.Empty;

		[MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
		public string? Description { get; set; }
	}
}
