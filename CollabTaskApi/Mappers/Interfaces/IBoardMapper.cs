using CollabTaskApi.DTOs.Board;

namespace CollabTaskApi.Mappers.Interfaces
{
	public interface IBoardMapper
	{
		BoardDto Map(
			BoardUserDto user,
			IEnumerable<BoardDeskDto> desks,
			IEnumerable<BoardDeskInvitationDto> invitations);
	}
}
