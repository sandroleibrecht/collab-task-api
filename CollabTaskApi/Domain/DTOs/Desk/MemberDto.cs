namespace CollabTaskApi.Domain.DTOs.Desk
{
	public class MemberDto
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string UserDeskRole { get; set; } = string.Empty;
	}
}
