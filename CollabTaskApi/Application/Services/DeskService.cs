using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.DTOs.Desk;
using CollabTaskApi.Infrastructure.Data;
using CollabTaskApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using CollabTaskApi.Application.Interfaces;

namespace CollabTaskApi.Application.Services
{
	public class DeskService(AppDbContext context) : IDeskService
	{
		private readonly AppDbContext _context = context;
		private const string DefaultDeskColorHex = "#FFFFFF";

		public async Task<IEnumerable<BoardDeskDto>> GetBoardDeskDtos(int userId)
		{
			var dtos = await (
				from ud in _context.UserDesks
				join d in _context.Desks on ud.DeskId equals d.Id
				join udt in _context.UserDeskTypes on ud.UserDeskTypeId equals udt.Id
				where ud.UserId == userId
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

				var userDesk = new UserDesk
				{
					UserId = userId,
					DeskId = desk.Id,
					UserDeskRoleId = userDeskRole.Id,
					UserDeskTypeId = userDeskType.Id,
					CreatedAt = now
				};

				await _context.UserDesks.AddAsync(userDesk);
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
	}
}
