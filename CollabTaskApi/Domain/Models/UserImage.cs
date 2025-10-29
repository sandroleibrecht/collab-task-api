namespace CollabTaskApi.Domain.Models
{
	public class UserImage
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string FilePath { get; set; } = string.Empty;

		public User User { get; set; } = null!;
	}
}
