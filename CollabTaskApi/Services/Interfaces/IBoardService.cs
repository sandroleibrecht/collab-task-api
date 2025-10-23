using CollabTaskApi.DTOs.Board;

namespace CollabTaskApi.Services.Interfaces
{
	public interface IBoardService
	{
		Task<BoardDto?> GetBoardDto(int userId);
	}
}
