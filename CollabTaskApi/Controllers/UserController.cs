using CollabTaskApi.Application.Interfaces;
using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.DTOs.User;
using CollabTaskApi.Shared.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollabTaskApi.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class UserController(IUserService userService, IValidator<UserUpdateDto> updateValidator) : ControllerBase
	{
		private readonly IUserService _userService = userService;
		private readonly IValidator<UserUpdateDto> _updateValidator = updateValidator;

		[HttpPut]
		public async Task<ActionResult<BoardUserDto>> Update([FromBody] UserUpdateDto dto)
		{
			var userId = User.GetUserId();
			if (userId is null) return Unauthorized();

			var validation = await _updateValidator.ValidateAsync(dto);
			if (!validation.IsValid) return BadRequest(validation.Errors);

			var updated = await _userService.UpdateAsync((int)userId, dto);
			if (updated is null) return BadRequest();

			return Ok(updated);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete()
		{
			var userId = User.GetUserId();
			if (userId is null) return Unauthorized();

			await _userService.DeleteAsync((int)userId);

			return NoContent();
		}
	}
}
