namespace CollabTaskApi.Domain.Models
{
	public class Lane
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public int Order {  get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
