using CollabTaskApi.DTOs.Board;
using CollabTaskApi.Extensions;
using CollabTaskApi.Services.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollabTaskApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class BoardController(IBoardService boardService) : ControllerBase
	{
		private readonly IBoardService _boardService = boardService;

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<BoardDto>(StatusCodes.Status200OK)]
		public async Task<ActionResult<BoardDto>> GetBoardDto()
		{
			var userId = User.GetUserId();
			if (userId == null) return Unauthorized();

			var boardDto = await _boardService.GetBoardDto((int)userId);
			if (boardDto is null) return BadRequest();

			return Ok(boardDto);
		}
	}
}
