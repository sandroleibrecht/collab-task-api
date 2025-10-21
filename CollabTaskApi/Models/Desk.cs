namespace CollabTaskApi.Models
{
	public class Desk
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Color { get; set; } = string.Empty;
		public int AdminId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
