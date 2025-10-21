namespace CollabTaskApi.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public int? SessionId { get; set; }
		public int? ImageId { get; set; }
		public DateTime CreatedAt { get; set; }
	} 
}
