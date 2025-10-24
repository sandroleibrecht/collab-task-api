using CollabTaskApi.DTOs.Board;
using CollabTaskApi.Mappers.Interfaces;

namespace CollabTaskApi.Mappers
{
	public class BoardMapper : IBoardMapper
	{
		public BoardDto Map(
			BoardUserDto user,
			IEnumerable<BoardDeskDto> desks,
			IEnumerable<BoardDeskInvitationDto> invitations)
		{
			return new BoardDto
			{
				User = user,
				OwnedDesks = [.. desks.Where(d => d.UserDeskType == "Owned")],
				SharedDesks = [.. desks.Where(d => d.UserDeskType == "Shared")],
				Invitations = [.. invitations]
			};
		}
	}
}
