namespace CollabTaskApi.Models
{
	public class UserDesk
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int DeskId { get; set; }
		public int UserDeskRoleId { get; set; }
		public int UserDeskTypeId { get; set; }
		public DateTime CreatedAt { get; set; }

		//public User? User { get; set; }
		//public Desk? Desk { get; set; }
		//public UserDeskRole? UserDeskRole { get; set; }
		//public UserDeskType? UserDeskType { get; set; }

		// TBD
	}
}
