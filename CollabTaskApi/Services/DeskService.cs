using CollabTaskApi.Data;
using CollabTaskApi.DTOs.Board;
using CollabTaskApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Services
{
	public class DeskService : IDeskService
	{
		private readonly AppDbContext _context;
		
		public DeskService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<BoardDeskDto>> GetBoardDeskDtos(int userId)
		{
			var dtos = await (
				from ud in _context.UserDesks
				join d in _context.Desks on ud.DeskId equals d.Id
				join udt in _context.UserDeskTypes on ud.UserDeskTypeId equals udt.Id
				where ud.UserId == userId
				select new BoardDeskDto
				{
					Id = d.Id,
					Name = d.Name,
					Color = d.Color,
					CreatedAt = d.CreatedAt,
					UserDeskType = udt.Name
				})
				.AsNoTracking()
				.ToListAsync();

			return dtos;
		}
	}
}
