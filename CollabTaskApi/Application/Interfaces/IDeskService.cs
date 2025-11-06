using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.DTOs.Desk;
using CollabTaskApi.Domain.Models;

namespace CollabTaskApi.Application.Interfaces
{
	public interface IDeskService
	{
		Task<Desk?> GetByIdAsync(int deskId);
		Task<DeskDto> GetDeskViewAsync(int userId, int deskId);
		Task<IEnumerable<Desk>> GetAllDesksAsync(int userId);
		Task<IEnumerable<BoardDeskDto>> GetBoardDeskDtosAsync(int userId);
		Task<BoardDeskDto> CreateAsync(int userId, CreateDeskDto dto);
		Task<BoardDeskDto> AddUserToDeskAsync(int userId, int deskId);
		Task RemoveUserFromDeskAsync(int userId, int deskId);
	}
}