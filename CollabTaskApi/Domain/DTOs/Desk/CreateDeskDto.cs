namespace CollabTaskApi.Domain.DTOs.Desk
{
	public class CreateDeskDto
	{
		public string Name { get; set; } = string.Empty;
		public string? Color { get; set; }
	}
}
