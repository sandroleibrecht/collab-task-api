using CollabTaskApi.Application.Interfaces;
using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.DTOs.User;
using CollabTaskApi.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollabTaskApi.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class UserController(IUserService userService) : ControllerBase
	{
		private readonly IUserService _userService = userService;

		[HttpPut]
		public async Task<ActionResult<BoardUserDto>> Update([FromBody] UserUpdateDto dto)
		{
			var userId = User.GetUserId();
			if (userId is null) return Unauthorized();

			//
			// validate userupdatedto
			//

			var updated = await _userService.UpdateAsync((int)userId, dto);
			if (updated is null) return BadRequest();

			return Ok(updated);
		}
	}
}
