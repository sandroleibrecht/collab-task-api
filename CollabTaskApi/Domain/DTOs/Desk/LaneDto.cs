namespace CollabTaskApi.Domain.DTOs.Desk
{
	public class LaneDto
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public int Order { get; set; }
		public DateTime CreatedAt { get; set; }
		public int DeskId { get; set; }
		public List<CardDto> Cards { get; set; } = [];
	}
}
