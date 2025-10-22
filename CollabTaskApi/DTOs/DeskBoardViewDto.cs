namespace CollabTaskApi.DTOs
{
	public class DeskBoardViewDto
	{
		public string Name { get; set; } = string.Empty;
		public string Color { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
		public string UserDeskType { get; set; } = string.Empty;
	}
}
