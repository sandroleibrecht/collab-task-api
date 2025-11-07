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
		ILogger<DeskService> logger) : IDeskService
	{
		private readonly AppDbContext _context = context;
		private readonly ILogger<DeskService> _logger = logger;
		private const string DefaultDeskColorHex = "#FFF";

		public async Task<Desk?> GetByIdAsync(int deskId)
		{
			return await _context.Desks.FirstOrDefaultAsync(d => d.Id == deskId);
		}

		public async Task<DeskDto> GetDeskViewAsync(int userId, int deskId)
		{
			var desk = await GetByIdAsync(deskId) ?? throw new Exception("Desk not found");

			var deskMembers = await _context.DeskUsers
				.Include(du => du.User)
				.Include(du => du.UserDeskRole)
				.Where(du => du.DeskId == desk.Id)
				.ToListAsync();

			if (!deskMembers.Any(dm => dm.UserId == userId)) throw new Exception("User is not a member of this desk");

			var lanes = await _context.Lanes
				.Select(lane => new LaneDto
				{
					Id = lane.Id,
					Order = lane.Order,
					CreatedAt = lane.CreatedAt,
					Cards = (
						from lc in _context.LaneCards
						join c in _context.Cards on lc.CardId equals c.Id
						where lc.LaneId == lane.Id
						select new CardDto
						{
							Id = c.Id,
							Name = c.Name,
							Description = c.Description,
							Order = c.Order,
							CreatedAt = c.CreatedAt,
							CardMembers = (
								from cu in _context.CardUsers
								join u in _context.Users on cu.UserId equals u.Id
								join du in _context.DeskUsers on u.Id equals du.UserId
								join udr in _context.UserDeskRoles on du.UserDeskRoleId equals udr.Id
								where cu.CardId == c.Id
								select new MemberDto
								{
									Id = u.Id,
									Name = u.Name,
									UserDeskRole = udr.Name
								}
							).ToList()
						}
					).ToList()
				})
				.ToListAsync();

			List <MemberDto> memberDtos = [];
			foreach(var member in deskMembers)
			{
				memberDtos.Add(new MemberDto
				{
					Id = member.Id,
					Name = member.User.Name,
					UserDeskRole = member.UserDeskRole.Name
				});
			}

			return new DeskDto
			{
				Id = desk.Id,
				Name = desk.Name,
				Lanes = lanes,
				Members = memberDtos
			};
		}

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

		public async Task<IEnumerable<BoardDeskDto>> GetBoardDeskDtosAsync(int userId)
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

		public async Task<BoardDeskDto> AddUserToDeskAsync(int userId, int deskId)
		{
			var desk = await _context.Desks.SingleOrDefaultAsync(d => d.Id == deskId)
			?? throw new Exception("Desk not found");

			var userExists = await _context.Users.AnyAsync(d => d.Id == userId);
			if (!userExists) throw new Exception("User not found");

			var alreadyMember = await _context.DeskUsers.AnyAsync(du => du.DeskId == deskId && du.UserId == userId);
			if (alreadyMember) throw new ArgumentException("User cannot be added to the desk: User is already a member");

			var userDeskRole = await _context.UserDeskRoles.FirstOrDefaultAsync(r => r.Name == "Member");
			var userDeskType = await _context.UserDeskTypes.FirstOrDefaultAsync(t => t.Name == "Shared");
			if (userDeskRole is null || userDeskType is null)
			{
				throw new InvalidOperationException("Error while getting the user deskrole/desktype.");
			}

			var deskUser = new DeskUser
			{
				DeskId = deskId,
				UserId = userId,
				UserDeskRoleId = userDeskRole.Id,
				UserDeskTypeId = userDeskType.Id,
				CreatedAt = DateTime.UtcNow
			};

			await _context.DeskUsers.AddAsync(deskUser);
			await _context.SaveChangesAsync();

			return new BoardDeskDto
			{
				DeskId = deskId,
				Name = desk.Name,
				Color = desk.Color,
				CreatedAt = DateTime.UtcNow,
				UserDeskType = userDeskType.Name
			};
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

				await _context.DeskInvitations.Where(i => i.DeskId == deskId).ExecuteDeleteAsync();
				
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
