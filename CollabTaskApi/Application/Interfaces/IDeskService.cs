using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.DTOs.Desk;

namespace CollabTaskApi.Application.Interfaces
{
	public interface IDeskService
	{
		Task<IEnumerable<BoardDeskDto>> GetBoardDeskDtos(int userId);
		Task<BoardDeskDto> CreateAsync(int userId, CreateDeskDto dto);
	}
}