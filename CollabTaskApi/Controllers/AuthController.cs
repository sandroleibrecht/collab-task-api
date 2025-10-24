using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.DTOs.User;
using CollabTaskApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace CollabTaskApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(IUserService userService) : ControllerBase
	{
		private readonly IUserService _userService = userService;

		[HttpPost("signup")]
		public async Task<ActionResult<UserDto>> SignUp(
			[FromBody] SignUpDto dto,
			[FromServices] IValidator validator)
		{
			// add fluentValidation
			// var validator = ...
			var createdUser = await _userService.CreateAsync(dto);
			if (createdUser is null) return BadRequest();
			return Ok(createdUser);
		}

		//[HttpPost("signin")]
		//public async Task<ActionResult<UserDto>> SignIn([FromBody] SignInDto dto)
		//{

		//}
	}
}
