using Microsoft.EntityFrameworkCore;
using CollabTaskApi.Domain.Models;
using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.DTOs.Desk;
using CollabTaskApi.Infrastructure.Data;
using CollabTaskApi.Application.Interfaces;

namespace CollabTaskApi.Application.Services
{
	public class DeskService(
		AppDbContext context,
		IInviteService inviteService) : IDeskService
	{
		private readonly AppDbContext _context = context;
		private readonly IInviteService _inviteService = inviteService;
		private const string DefaultDeskColorHex = "#FFF";

		public async Task<IEnumerable<Desk>> GetAllDesksAsync(int userId)
		{
			var desks = await (
				from d in _context.Desks
				join du in _context.DeskUsers on d.Id equals du.DeskId
				where du.UserId == userId
				select d
			)
			.ToListAsync();

			return desks;
		}

		public async Task<IEnumerable<BoardDeskDto>> GetBoardDeskDtos(int userId)
		{
			var dtos = await (
				from du in _context.DeskUsers
				join d in _context.Desks on du.DeskId equals d.Id
				join udt in _context.UserDeskTypes on du.UserDeskTypeId equals udt.Id
				where du.UserId == userId
				select new BoardDeskDto
				{
					DeskId = d.Id,
					Name = d.Name,
					Color = d.Color,
					CreatedAt = d.CreatedAt,
					UserDeskType = udt.Name
				})
				.AsNoTracking()
				.ToListAsync();

			return dtos;
		}

		public async Task<BoardDeskDto> CreateAsync(int userId, CreateDeskDto dto)
		{
			var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
			if (!userExists) throw new ArgumentException("Unable to find user with provided UserId.");

			using var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				var now = DateTime.UtcNow;

				var desk = new Desk
				{
					Name = dto.Name,
					Color = dto.Color ?? DefaultDeskColorHex,
					CreatedAt = now
				};

				await _context.Desks.AddAsync(desk);
				await _context.SaveChangesAsync();

				var userDeskRole = await _context.UserDeskRoles.FirstOrDefaultAsync(r => r.Name == "Admin");
				var userDeskType = await _context.UserDeskTypes.FirstOrDefaultAsync(t => t.Name == "Owned");
				if (userDeskRole is null || userDeskType is null)
				{
					throw new InvalidOperationException("Error while getting the user deskrole/desktype.");
				}

				var userDesk = new DeskUser
				{
					UserId = userId,
					DeskId = desk.Id,
					UserDeskRoleId = userDeskRole.Id,
					UserDeskTypeId = userDeskType.Id,
					CreatedAt = now
				};

				await _context.DeskUsers.AddAsync(userDesk);
				await _context.SaveChangesAsync();

				await transaction.CommitAsync();

				return new BoardDeskDto
				{
					DeskId = desk.Id,
					Name = desk.Name,
					Color = desk.Color,
					CreatedAt = desk.CreatedAt,
					UserDeskType = userDeskType.Name
				};
			}
			catch(Exception ex)
			{
				await transaction.RollbackAsync();
				throw new Exception(ex.Message);
			}
		}

		public async Task HandleUserLeaveAsync(int userId, Desk desk)
		{
			if (desk == null) return;

			var deskUsers = await _context.DeskUsers.Where(ud => ud.DeskId == desk.Id).ToListAsync();
			if (deskUsers.Count == 1)
			{
				await _inviteService.DeleteAllInvitationsByDeskIdAsync(desk.Id);
				await _context.DeskUsers.Where(ud => ud.Id == deskUsers[0].Id).ExecuteDeleteAsync();
				await _context.CardUsers.Where(cu => cu.UserId == userId).ExecuteDeleteAsync();

				var deskLanes = await _context.DeskLanes
					.Where(dl => dl.DeskId == desk.Id)
					.ToListAsync();

				var laneIds = deskLanes
					.Select(dl => dl.LaneId)
					.ToList();

				var lanes = await _context.Lanes
					.Where(l => laneIds.Contains(l.Id))
					.ToListAsync();

				var laneCards = await _context.LaneCards
					.Where(lc => laneIds.Contains(lc.LaneId))
					.ToListAsync();

				var cardIds = laneCards.Select(lc => lc.CardId).ToList();

				var cards = await _context.Cards
					.Where(c => cardIds.Contains(c.Id))
					.ToListAsync();

				await _context.DeskLanes.Where(dl => laneIds.Contains(dl.LaneId)).ExecuteDeleteAsync();
				await _context.LaneCards.Where(lc => laneIds.Contains(lc.LaneId)).ExecuteDeleteAsync();

				_context.Lanes.RemoveRange(lanes);
				_context.Cards.RemoveRange(cards);
				_context.Desks.Remove(desk);

				await _context.SaveChangesAsync();
			}
			else
			{
				// TBD HERE: handle leaving of desk with other members
			}
		}
	}
}
