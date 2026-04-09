using CollabTask.Api.Domain.DTOs.Board;

namespace CollabTask.Api.Application.Interfaces
{
	public interface IInviteService
	{
		Task<IEnumerable<BoardDeskInvitationDto>> GetBoardDeskInvitationDtos(int userId);
		Task SendInvitationAsync(int senderId, string receiverEmail, int deskId);
		Task<BoardDeskDto> AcceptInvitationAsync(int inviteId);
		Task DeclineInvitationAsync(int inviteId);
		Task DeleteAllInvitationsByUserIdAsync(int userId);
		Task DeleteAllInvitationsByDeskIdAsync(int deskId);
	}
}
