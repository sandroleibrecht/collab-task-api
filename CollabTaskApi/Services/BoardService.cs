using CollabTaskApi.Data;
using CollabTaskApi.DTOs.Board;
using CollabTaskApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Services
{
	public class BoardService : IBoardService
	{
		private readonly AppDbContext _context;
		private readonly IDeskService _deskService;
		private readonly IUserService _userService;
		private readonly IInviteService _inviteService;

		public BoardService(
			AppDbContext context,
			IDeskService deskService,
			IUserService userService,
			IInviteService inviteService)
		{
			_context = context;
			_deskService = deskService;
			_userService = userService;
			_inviteService = inviteService;
		}

		public async Task<BoardDto?> GetBoardDto(int userId)
		{
			var boardUser = await _userService.GetBoardUserDto(userId);
			if (boardUser == null) return null;

			var boardDesks = await _deskService.GetBoardDeskDtos(userId);
			var boardInvites = await _inviteService.GetBoardDeskInvitationDtos(userId);

			var boardDto = new BoardDto
			{
				User = boardUser,
				OwnedDesks = [.. boardDesks.Where(d => d.UserDeskType == "Owned")],
				SharedDesks = [.. boardDesks.Where(d => d.UserDeskType == "Shared")],
				Invitations = [.. boardInvites]
			};

			return boardDto;
		}


	}
}
