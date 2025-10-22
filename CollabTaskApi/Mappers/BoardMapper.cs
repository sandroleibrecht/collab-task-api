using CollabTaskApi.DTOs;
using CollabTaskApi.Models;

namespace CollabTaskApi.Mappers
{
	public static class BoardMapper
	{
		public static DeskBoardViewDto ToDeskBoardViewDto(Desk d)
		{
			return new DeskBoardViewDto
			{
				Name = d.Name,
				Color = d.Color,
				CreatedAt = d.CreatedAt,
				UserDeskType = "TBD" // d.UserDeskType.Name ??
			};
		}
	}
}
