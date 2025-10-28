namespace CollabTaskApi.Domain.Models
{
	public class DeskLane
	{
		public int Id { get; set; }
		public int DeskId { get; set; }
		public int LaneId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
