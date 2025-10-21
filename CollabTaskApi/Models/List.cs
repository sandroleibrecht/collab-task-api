namespace CollabTaskApi.Models
{
	public class List
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int Order {  get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
