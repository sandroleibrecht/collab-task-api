using CollabTaskApi.Models;

namespace CollabTaskApi.Services.Interfaces
{
	public interface IBoardService
	{
		Task<IEnumerable<Desk>> GetAllDesks(int userId);
	}
}
