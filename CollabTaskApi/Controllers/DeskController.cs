using CollabTaskApi.Application.Interfaces;
using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.DTOs.Desk;
using CollabTaskApi.Shared.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollabTaskApi.Controllers
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
		public async Task<ActionResult<DeskDto>> Get(int deskId)
		{
			var userId = User.GetUserId();
			if (userId is null) return Unauthorized();

			return new DeskDto
			{
				Id = deskId,
				Name = "Test"
			};
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
