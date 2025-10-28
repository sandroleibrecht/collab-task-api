using CollabTaskApi.Domain.DTOs.Board;

namespace CollabTaskApi.Application.Interfaces
{
	public interface IInviteService
	{
		Task<IEnumerable<BoardDeskInvitationDto>> GetBoardDeskInvitationDtos(int userId);
		Task DeleteAllInvitationsByUserIdAsync(int userId);
		Task DeleteAllInvitationsByDeskIdAsync(int deskId);
	}
}
