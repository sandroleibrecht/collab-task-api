namespace CollabTaskApi.DTOs.Board
{
	public class BoardDeskDto
	{
		public int DeskId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Color { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
		public string UserDeskType { get; set; } = string.Empty;
	}
}
