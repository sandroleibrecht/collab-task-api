using CollabTask.Shared.DTOs.Board;

namespace CollabTask.Api.Application.Interfaces
{
	public interface IBoardService
	{
		Task<BoardDto?> GetBoardDto(int userId);
	}
}
