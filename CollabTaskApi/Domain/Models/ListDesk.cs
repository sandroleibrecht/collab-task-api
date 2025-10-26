namespace CollabTaskApi.Domain.Models
{
	public class ListDesk
	{
		public int Id { get; set; }
		public int ListId { get; set; }
		public int DeskId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
