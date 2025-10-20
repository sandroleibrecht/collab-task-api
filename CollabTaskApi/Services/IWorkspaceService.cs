using CollabTaskApi.Models;

namespace CollabTaskApi.Services
{
	public interface IWorkspaceService
	{
		Task<IEnumerable<Workspace>> GetAll();
		Task<Workspace?> GetById(int id);
		Task<Workspace> Create(Workspace workspace);
		Task<bool> Delete(int id);
	}
}
