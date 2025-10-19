using System.ComponentModel.DataAnnotations;

namespace CollabTaskApi.DTOs
{
	public class WorkspaceCreateDto
	{
		[Required(ErrorMessage = "Name is required")]
		[MinLength(1, ErrorMessage = "Name must be at least 1 character long")]
		public string Name { get; set; } = "Unnamed Workspace";

		public string? Description { get; set; }
	}
}
