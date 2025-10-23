using CollabTaskApi.DTOs.Board;

namespace CollabTaskApi.Services.Interfaces
{
	public interface IDeskService
	{
		Task<IEnumerable<BoardDeskDto>> GetBoardDeskDtos(int userId);
	}
}
