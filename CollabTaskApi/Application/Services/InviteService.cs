using CollabTaskApi.Application.Interfaces;
using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.Models;
using CollabTaskApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Application.Services
{
	public class InviteService(AppDbContext context, IDeskService deskService) : IInviteService
	{
		private readonly AppDbContext _context = context;
		private readonly IDeskService _deskService = deskService;
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

		public async Task SendInvitationAsync(int senderId, string receiverEmail, int deskId)
		{
			var senderExists = await _context.Users.AnyAsync(u => u.Id == senderId);
			if(!senderExists) throw new Exception("Sending user not found");

			var receivingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == receiverEmail)
			?? throw new Exception("Receiving user not found");

			var deskExists = await _context.Desks.AnyAsync(d => d.Id == deskId);
			if (!deskExists) throw new Exception("Desk not found");

			var alreadyMember = await _context.DeskUsers.AnyAsync(du => du.DeskId == deskId && du.UserId == receivingUser.Id);
			if (alreadyMember) throw new Exception("Receiving user is already a desk member");

			var inviteAlreadySent = await _context.DeskInvitations.AnyAsync(i => i.DeskId == deskId && i.ReceiverUserId == receivingUser.Id);
			if (inviteAlreadySent) throw new Exception("User has already been invited");

			var deskInvitation = new DeskInvitation
			{
				SenderUserId = senderId,
				ReceiverUserId = receivingUser.Id,
				DeskId = deskId,
				CreatedAt = DateTime.UtcNow
			};

			await _context.DeskInvitations.AddAsync(deskInvitation);
			await _context.SaveChangesAsync();
		}

		public async Task<BoardDeskDto> AcceptInvitationAsync(int inviteId)
		{
			var invite = await _context.DeskInvitations.SingleOrDefaultAsync(i => i.Id == inviteId)
			?? throw new ArgumentException("Invitation not found");

			var desk = await _deskService.GetByIdAsync(invite.DeskId)
			?? throw new Exception("Desk not found");

			var deskType = await _context.UserDeskTypes.FirstOrDefaultAsync(t => t.Name == "Shared")
			?? throw new Exception("User desk type not found");

			await _deskService.AddUserToDeskAsync(invite.ReceiverUserId, desk.Id);

			_context.DeskInvitations.Remove(invite);
			await _context.SaveChangesAsync();

			return new BoardDeskDto
			{
				DeskId = desk.Id,
				Name = desk.Name,
				Color = desk.Color,
				CreatedAt = desk.CreatedAt,
				UserDeskType = deskType.Name
			};
		}

		public async Task DeclineInvitationAsync(int inviteId)
		{
			var invite = await _context.DeskInvitations.FirstOrDefaultAsync(i => i.Id == inviteId);

			if (invite is not null)
			{
				_context.DeskInvitations.Remove(invite);
				await _context.SaveChangesAsync();
			}
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
