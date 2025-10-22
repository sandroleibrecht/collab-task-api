using CollabTaskApi.DTOs;
using CollabTaskApi.Mappers;
using CollabTaskApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollabTaskApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BoardController : ControllerBase
	{
		private readonly IBoardService _service;

		public BoardController(IBoardService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<DeskBoardViewDto>>> GetAllDesks(int userId)
		{
			var deskModels = await _service.GetAllDesks(userId);
			var deskDtos = deskModels.Select(d => BoardMapper.ToDeskBoardViewDto(d));
			return Ok(deskDtos);
		}
	}
}
