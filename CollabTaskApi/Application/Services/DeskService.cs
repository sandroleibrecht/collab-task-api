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
		ILogger<DeskService> logger,
		IInviteService inviteService) : IDeskService
	{
		private readonly AppDbContext _context = context;
		private readonly ILogger<DeskService> _logger = logger;
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

		public async Task RemoveUserFromDeskAsync(int userId, int deskId)
		{
			_logger.LogInformation("Removing User (Id: {UserId}) from Desk (Id: {DeskId})...", userId, deskId);

			var deskUsers = await _context.DeskUsers
				.Include(du => du.UserDeskRole)
				.Where(du => du.DeskId == deskId).ToListAsync();

			if (deskUsers.Count == 1 && deskUsers[0].UserId == userId)
			{
				_logger.LogInformation("Provided user is the last member of this Desk. Initiate desk deletion...");
				await DeleteEntireDeskAsync(deskId);
				return;
			}

			var deskUserToBeRemoved = deskUsers.FirstOrDefault(du => du.UserId == userId)
			?? throw new Exception("Provided user is not a member of this desk");

			var cardUsers = await (
				from cu in _context.CardUsers
				join lc in _context.LaneCards on cu.CardId equals lc.CardId
				join dl in _context.DeskLanes on lc.LaneId equals dl.LaneId
				where dl.DeskId == deskId && cu.UserId == userId
				select cu.Id
			).ToListAsync();

			_logger.LogInformation("Removing user from {Count} Cards...", cardUsers.Count);
			await _context.CardUsers.Where(cu => cardUsers.Contains(cu.Id)).ExecuteDeleteAsync();

			if (deskUserToBeRemoved.UserDeskRole.Name == "Admin")
			{
				var otherMembers = await _context.DeskUsers
					.Include(du => du.UserDeskRole)
					.Where(du => du.DeskId == deskId && du.UserId != userId)
					.ToListAsync();

				var admins = otherMembers.Where(du => du.UserDeskRole.Name == "Admin").ToList();

				if (admins.Count == 0)
				{
					_logger.LogInformation("User to be removed is of role type 'Admin'. Transfering admin role...");

					var randomIndex = Random.Shared.Next(0, otherMembers.Count);
					var newAdmin = otherMembers[randomIndex];
					newAdmin.UserDeskRoleId = deskUserToBeRemoved.UserDeskRoleId;

					_logger.LogInformation("Admin role transfered to other desk member (Id: {UserId})", newAdmin.UserId);
				}
			}

			_context.DeskUsers.Remove(deskUserToBeRemoved);
			await _context.SaveChangesAsync();
		}

		private async Task DeleteEntireDeskAsync(int deskId)
		{
			_logger.LogInformation("Deleting Desk (Id: {DeskId}) and all related entities...", deskId);
			
			using var transaction = await _context.Database.BeginTransactionAsync();
			try
			{
				var cardIds = await (
					from c in _context.Cards
					join lc in _context.LaneCards on c.Id equals lc.CardId
					join l in _context.Lanes on lc.LaneId equals l.Id
					join dl in _context.DeskLanes on l.Id equals dl.LaneId
					where dl.DeskId == deskId
					select c.Id
				).ToListAsync();

				_logger.LogInformation("Deleting {Count} Cards...", cardIds.Count);
				await _context.CardUsers.Where(cu => cardIds.Contains(cu.CardId)).ExecuteDeleteAsync();
				await _context.LaneCards.Where(lc => cardIds.Contains(lc.CardId)).ExecuteDeleteAsync();
				await _context.Cards.Where(c => cardIds.Contains(c.Id)).ExecuteDeleteAsync();

				var laneIds = await (
					from l in _context.Lanes
					join dl in _context.DeskLanes on l.Id equals dl.LaneId
					where dl.DeskId == deskId
					select l.Id
				).ToListAsync();

				_logger.LogInformation("Deleting {Count} Lanes...", laneIds.Count);
				await _context.DeskLanes.Where(dl => dl.DeskId == deskId).ExecuteDeleteAsync();
				await _context.Lanes.Where(l => laneIds.Contains(l.Id)).ExecuteDeleteAsync();

				_logger.LogInformation("Deleting Invitations...");

				await _inviteService.DeleteAllInvitationsByDeskIdAsync(deskId);

				_logger.LogInformation("Deleting Desk...");

				await _context.DeskUsers.Where(ud => ud.DeskId == deskId).ExecuteDeleteAsync();
				await _context.Desks.Where(d => d.Id == deskId).ExecuteDeleteAsync();

				await _context.SaveChangesAsync();
				await transaction.CommitAsync();
			}
			catch (Exception ex)
			{
				_logger.LogInformation(ex, "Error while deleting Desk (Id: {DeskId})", deskId);
				await transaction.RollbackAsync();
				throw new Exception("Unable to delete Desk");
			}
		}

		
	}
}
