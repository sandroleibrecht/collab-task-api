namespace CollabTaskApi.Domain.Models
{
	public class UserDesk
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int DeskId { get; set; }
		public int UserDeskRoleId { get; set; }
		public int UserDeskTypeId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
