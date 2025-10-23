namespace CollabTaskApi.DTOs.Board
{
	public class BoardDto
	{
		public BoardUserDto User { get; set; } = new();
		public List<BoardDeskDto> OwnedDesks { get; set; } = [];
		public List<BoardDeskDto> SharedDesks { get; set; } = [];
		public List<BoardDeskInvitationDto> Invitations { get; set; } = [];
	}
}
