namespace CollabTaskApi.Models
{
	public class TaskList
	{
		public int Id { get; set; }
		public int TaskId { get; set; }
		public int ListId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
