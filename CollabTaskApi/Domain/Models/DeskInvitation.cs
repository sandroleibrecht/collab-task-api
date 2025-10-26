namespace CollabTaskApi.Domain.Models
{
	public class DeskInvitation
	{
		public int Id { get; set; }
		public int SenderUserId { get; set; }
		public int ReceiverUserId { get; set; }
		public int DeskId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
