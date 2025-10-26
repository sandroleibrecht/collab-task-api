using CollabTaskApi.DTOs.Board;

namespace CollabTaskApi.Services.Application.Interfaces
{
	public interface IBoardService
	{
		Task<BoardDto?> GetBoardDto(int userId);
	}
}
