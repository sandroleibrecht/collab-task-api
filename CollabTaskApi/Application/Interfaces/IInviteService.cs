using CollabTaskApi.Domain.DTOs.Board;

namespace CollabTaskApi.Application.Interfaces
{
	public interface IInviteService
	{
		Task<IEnumerable<BoardDeskInvitationDto>> GetBoardDeskInvitationDtos(int userId);
		Task<BoardDeskDto> AcceptInvitationAsync(int inviteId);
		Task DeclineInvitationAsync(int inviteId);
		Task DeleteAllInvitationsByUserIdAsync(int userId);
		Task DeleteAllInvitationsByDeskIdAsync(int deskId);
	}
}
