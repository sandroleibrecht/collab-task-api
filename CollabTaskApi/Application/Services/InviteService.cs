using CollabTaskApi.Application.Interfaces;
using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Application.Services
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

		public async Task<BoardDeskDto> AcceptInvitationAsync(int inviteId)
		{
			return new BoardDeskDto();
		}

		public async Task DeclineInvitationAsync(int inviteId)
		{

		}

		public async Task DeleteAllInvitationsByUserIdAsync(int userId)
		{
			var invites = await _context.DeskInvitations
				.Where(i => i.ReceiverUserId == userId || i.SenderUserId == userId)
				.ToListAsync();

			_context.RemoveRange(invites);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAllInvitationsByDeskIdAsync(int deskId)
		{
			var invites = await _context.DeskInvitations
				.Where(i => i.DeskId == deskId)
				.ToListAsync();

			_context.RemoveRange(invites);
			await _context.SaveChangesAsync();
		}
	}
}
