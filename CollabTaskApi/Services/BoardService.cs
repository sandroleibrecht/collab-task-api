using CollabTaskApi.Data;
using CollabTaskApi.Models;
using CollabTaskApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Services
{
	public class BoardService : IBoardService
	{
		private readonly AppDbContext _context;

		public BoardService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Desk>> GetAllDesks(int userId)
		{
			return await _context.Desks.AsNoTracking().ToListAsync();
		}
	}
}
