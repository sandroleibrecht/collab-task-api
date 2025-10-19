using CollabTaskApi.Data;
using CollabTaskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CollabTaskApi.Services
{
	public class WorkspaceService : IWorkspaceService
	{
		private readonly AppDbContext _context;

		public WorkspaceService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Workspace>> GetAll()
		{
			var workspaces = await _context.Workspaces.AsNoTracking().ToListAsync();
			return workspaces;
		}

		public async Task<Workspace> Create(Workspace workspace)
		{
			await _context.AddAsync(workspace);
			await _context.SaveChangesAsync();
			return workspace;
		}

		public async Task<bool> Delete(int id)
		{
			var workspace = await _context.Workspaces.AsNoTracking().SingleOrDefaultAsync(w => w.Id == id);
			if (workspace == null) return false;
			_context.Workspaces.Remove(workspace);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
