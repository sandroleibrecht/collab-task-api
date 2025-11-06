using CollabTaskApi.Domain.Models;

namespace CollabTaskApi.Domain.DTOs.Desk
{
	public class CardDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public int Order { get; set; }
		public DateTime CreatedAt { get; set; }
		public List<MemberDto> CardMembers { get; set; } = [];
	}
}
