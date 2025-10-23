namespace CollabTaskApi.DTOs.Board
{
	public class BoardUserDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string? ImagePath { get; set; } = string.Empty;
	}
}
