using CollabTaskApi.DTOs.Board;
using CollabTaskApi.Mappers.Interfaces;
using CollabTaskApi.Services.Interfaces;

namespace CollabTaskApi.Services
{
	public class BoardService(
		IBoardMapper boardMapper,
		IDeskService deskService,
		IUserService userService,
		IInviteService inviteService) : IBoardService
	{
		private readonly IBoardMapper _boardMapper = boardMapper;
		private readonly IDeskService _deskService = deskService;
		private readonly IUserService _userService = userService;
		private readonly IInviteService _inviteService = inviteService;

		public async Task<BoardDto?> GetBoardDto(int userId)
		{
			var boardUser = await _userService.GetBoardUserDtoAsync(userId);

			if (boardUser == null)
			{
				return null;
			}

			var boardDesks = await _deskService.GetBoardDeskDtos(userId);
			var boardInvites = await _inviteService.GetBoardDeskInvitationDtos(userId);

			return _boardMapper.Map(boardUser, boardDesks, boardInvites);
		}
	}
}
