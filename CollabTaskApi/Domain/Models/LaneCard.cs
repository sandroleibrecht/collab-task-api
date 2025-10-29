namespace CollabTaskApi.Domain.Models
{
	public class LaneCard
	{
		public int Id { get; set; }
		public int LaneId { get; set; }
		public int CardId { get; set; }
		public DateTime CreatedAt { get; set; }

		public Lane Lane { get; set; } = null!;
		public Card Card { get; set; } = null!;
	}
}
