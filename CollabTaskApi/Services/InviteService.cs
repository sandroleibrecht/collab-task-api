using CollabTaskApi.Data;
using CollabTaskApi.DTOs.Board;
using CollabTaskApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Services
{
	public class InviteService(AppDbContext context) : IInviteService
	{
		private readonly AppDbContext _context = context;

		public async Task<IEnumerable<BoardDeskInvitationDto>> GetBoardDeskInvitationDtos(int userId)
		{
			var dtos = await (
				from di in _context.DeskInvitations
				join u in _context.Users on di.SenderUserId equals u.Id
				join d in _context.Desks on di.DeskId equals d.Id
				where di.ReceiverUserId == userId
				select new BoardDeskInvitationDto
				{
					Id = di.Id,
					SenderName = u.Name,
					DeskName = d.Name,
					CreatedAt = di.CreatedAt
				}
			)
			.AsNoTracking()
			.ToListAsync();

			return dtos;
		}
	}
}
