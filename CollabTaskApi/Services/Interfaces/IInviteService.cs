using CollabTaskApi.DTOs.Board;

namespace CollabTaskApi.Services.Interfaces
{
	public interface IInviteService
	{
		Task<IEnumerable<BoardDeskInvitationDto>> GetBoardDeskInvitationDtos(int userId);
	}
}
