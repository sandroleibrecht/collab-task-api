namespace CollabTaskApi.Domain.DTOs.Invites
{
	public class SendInvitationDto
	{
		public string ReceiverEmail { get; set; } = string.Empty;
		public int DeskId { get; set; }
	}
}
