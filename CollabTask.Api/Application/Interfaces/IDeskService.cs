using CollabTask.Shared.DTOs.Board;
using CollabTask.Shared.DTOs.Desk;
using CollabTask.Api.Domain.Models;

namespace CollabTask.Api.Application.Interfaces
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