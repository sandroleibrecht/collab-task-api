using CollabTask.Api.Application.Interfaces;
using CollabTask.Shared.DTOs.Board;
using CollabTask.Api.Domain.DTOs.Desk;
using CollabTask.Api.Shared.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollabTask.Api.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class DeskController(
		IValidator<CreateDeskDto> createDeskValidator,
		IDeskService deskService) : ControllerBase
	{
		private readonly IValidator<CreateDeskDto> _createDeskValidator = createDeskValidator;
		private readonly IDeskService _deskService = deskService;

		[HttpGet("{deskId:int}")]
		public async Task<ActionResult<DeskDto>> GetDeskView(int deskId)
		{
			var userId = User.GetUserId();
			if (userId is null) return Unauthorized();

			var deskView = await _deskService.GetDeskViewAsync((int)userId, deskId);

			return deskView;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<List<ValidationFailure>>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<BoardDeskDto>(StatusCodes.Status200OK)]
		public async Task<ActionResult<BoardDeskDto>> Create([FromBody] CreateDeskDto dto)
		{
			var userId = User.GetUserId();
			if (userId is null) return Unauthorized();
			
			var validation = await _createDeskValidator.ValidateAsync(dto);
			if (!validation.IsValid) return BadRequest(validation.Errors);

			var createdDesk = await _deskService.CreateAsync((int)userId, dto);
			
			return Ok(createdDesk);
		}
	}
}
