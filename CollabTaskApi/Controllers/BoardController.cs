using CollabTaskApi.DTOs.Board;
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

		[HttpGet("{userId:int}")]
		public async Task<ActionResult<BoardDto?>> GetBoardDto(int userId)
		{
			var boardDto = await _service.GetBoardDto(userId);
			if (boardDto is null) return BadRequest();
			return Ok(boardDto);
		}
	}
}
