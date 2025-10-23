namespace CollabTaskApi.DTOs.Board
{
	public class BoardDeskInvitationDto
	{
		public int Id { get; set; }
		public string SenderName { get; set; } = string.Empty;
		public string DeskName { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
	}
}
