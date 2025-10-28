using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.DTOs.Desk;
using CollabTaskApi.Domain.Models;

namespace CollabTaskApi.Application.Interfaces
{
	public interface IDeskService
	{
		Task<IEnumerable<Desk>> GetAllDesksAsync(int userId);
		Task<IEnumerable<BoardDeskDto>> GetBoardDeskDtos(int userId);
		Task<BoardDeskDto> CreateAsync(int userId, CreateDeskDto dto);
		Task HandleUserLeaveAsync(int userId, Desk desk);
	}
}