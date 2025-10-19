using CollabTaskApi.Data;
using CollabTaskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Services
{
	public class UserService : IUserService
	{
		private readonly AppDbContext _context;

		public UserService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<User>> GetAll()
		{
			return await _context.Users.AsNoTracking().ToListAsync();
		}

		public async Task<User> Create(User user)
		{
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();
			return user;
		}

		public async Task<bool> Delete(int id)
		{
			var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

			if (user == null) return false;

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
