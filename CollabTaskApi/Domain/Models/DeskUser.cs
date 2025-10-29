namespace CollabTaskApi.Domain.Models
{
	public class DeskUser
	{
		public int Id { get; set; }
		public int DeskId { get; set; }
		public int UserId { get; set; }
		public int UserDeskRoleId { get; set; }
		public int UserDeskTypeId { get; set; }
		public DateTime CreatedAt { get; set; }

		public Desk Desk { get; set; } = null!;
		public User User { get; set; } = null!;
		public UserDeskRole UserDeskRole { get; set; } = null!;
		public UserDeskType UserDeskType { get; set; } = null!;
	}
}
