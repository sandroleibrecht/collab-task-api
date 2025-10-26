using CollabTaskApi.DTOs.Api.Errors;
using CollabTaskApi.DTOs.Board;
using CollabTaskApi.DTOs.Desk;
using CollabTaskApi.Helpers.Auth;
using CollabTaskApi.Mappers.Interfaces;
using CollabTaskApi.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollabTaskApi.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class DeskController(
		IValidator<CreateDeskDto> createDeskValidator,
		IErrorMapper errorMapper,
		IDeskService deskService) : ControllerBase
	{
		private readonly IValidator<CreateDeskDto> _createDeskValidator = createDeskValidator;
		private readonly IErrorMapper _errorMapper = errorMapper;
		private readonly IDeskService _deskService = deskService;

		[HttpPost]
		[ProducesResponseType(typeof(ApiErrorDto), StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(IEnumerable<ValidationErrorDto>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(BoardDeskDto), StatusCodes.Status200OK)]
		public async Task<ActionResult<BoardDeskDto>> Create([FromBody] CreateDeskDto dto)
		{
			var userId = User.GetUserId();
			if (userId is null) return Unauthorized(new ApiErrorDto { Type = "AuthError", Message = "Unable to authorize user."});
			
			var validation = await _createDeskValidator.ValidateAsync(dto);
			if (!validation.IsValid) return BadRequest(_errorMapper.Map(validation.Errors));

			var createdDesk = await _deskService.CreateAsync((int)userId, dto);
			return Ok(createdDesk);
		}
	}
}
