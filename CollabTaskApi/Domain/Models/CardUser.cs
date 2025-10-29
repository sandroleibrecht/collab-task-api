namespace CollabTaskApi.Domain.Models
{
	public class CardUser
	{
		public int Id { get; set; }
		public int CardId { get; set; }
		public int UserId { get; set; }
		public DateTime CreatedAt { get; set; }

		public Card Card { get; set; } = null!;
		public User User { get; set; } = null!;
	}
}
