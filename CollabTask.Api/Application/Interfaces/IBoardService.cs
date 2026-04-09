using CollabTask.Api.Domain.DTOs.Board;

namespace CollabTask.Api.Application.Interfaces
{
	public interface IBoardService
	{
		Task<BoardDto?> GetBoardDto(int userId);
	}
}
