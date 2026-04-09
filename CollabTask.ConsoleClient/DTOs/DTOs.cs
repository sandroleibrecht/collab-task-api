namespace CollabTask.ConsoleClient.DTOs
{
	public class DeskDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Color { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
	}

	public class BoardDto
	{
		public BoardUserDto User { get; set; } = new();
		public List<BoardDeskDto> OwnedDesks { get; set; } = [];
		public List<BoardDeskDto> SharedDesks { get; set; } = [];
		public List<BoardDeskInvitationDto> Invitations { get; set; } = [];
	}

	public class BoardUserDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string? ImagePath { get; set; } = string.Empty;
	}

	public class BoardDeskDto
	{
		public int DeskId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Color { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
		public string UserDeskType { get; set; } = string.Empty;
	}

	public class BoardDeskInvitationDto
	{
		public int Id { get; set; }
		public string SenderName { get; set; } = string.Empty;
		public string DeskName { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
	}
}
