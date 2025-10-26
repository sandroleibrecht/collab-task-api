namespace CollabTaskApi.Domain.Models
{
	public class UserTask
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int TaskId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
