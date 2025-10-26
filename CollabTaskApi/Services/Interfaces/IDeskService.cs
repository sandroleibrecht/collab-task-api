using CollabTaskApi.DTOs.Board;
using CollabTaskApi.DTOs.Desk;

namespace CollabTaskApi.Services.Interfaces
{
	public interface IDeskService
	{
		Task<IEnumerable<BoardDeskDto>> GetBoardDeskDtos(int userId);
		Task<BoardDeskDto> CreateAsync(int userId, CreateDeskDto dto);
	}
}