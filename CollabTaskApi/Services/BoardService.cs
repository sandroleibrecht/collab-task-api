using CollabTaskApi.DTOs.Board;
using CollabTaskApi.Services.Interfaces;

namespace CollabTaskApi.Services
{
	public class BoardService(
		IDeskService deskService,
		IUserService userService,
		IInviteService inviteService) : IBoardService
	{
		private readonly IDeskService _deskService = deskService;
		private readonly IUserService _userService = userService;
		private readonly IInviteService _inviteService = inviteService;

		public async Task<BoardDto?> GetBoardDto(int userId)
		{
			var boardUser = await _userService.GetBoardUserDtoAsync(userId);
			if (boardUser == null) return null;

			var boardDesks = await _deskService.GetBoardDeskDtos(userId);
			var boardInvites = await _inviteService.GetBoardDeskInvitationDtos(userId);

			return new BoardDto
			{
				User = boardUser,
				OwnedDesks = [.. boardDesks.Where(d => d.UserDeskType == "Owned")],
				SharedDesks = [.. boardDesks.Where(d => d.UserDeskType == "Shared")],
				Invitations = [.. boardInvites]
			};
		}
	}
}
