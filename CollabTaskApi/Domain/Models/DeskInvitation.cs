namespace CollabTaskApi.Domain.Models
{
	public class DeskInvitation
	{
		public int Id { get; set; }
		public int SenderUserId { get; set; }
		public int ReceiverUserId { get; set; }
		public int DeskId { get; set; }
		public DateTime CreatedAt { get; set; }

		public User SenderUser { get; set; } = null!;
		public User ReceiverUser { get; set; } = null!;
		public Desk Desk { get; set; } = null!;
	}
}
