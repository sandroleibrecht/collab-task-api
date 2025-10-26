using CollabTaskApi.Domain.DTOs.Board;

namespace CollabTaskApi.Application.Interfaces
{
	public interface IBoardService
	{
		Task<BoardDto?> GetBoardDto(int userId);
	}
}
