namespace CollabTaskApi.Domain.Models
{
	public class LaneCard
	{
		public int Id { get; set; }
		public int LaneId { get; set; }
		public int CardId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
