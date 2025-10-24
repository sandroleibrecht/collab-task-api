using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.DTOs.User;
using CollabTaskApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace CollabTaskApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(IValidator<SignUpDto> validator, IUserService userService) : ControllerBase
	{
		private readonly IValidator<SignUpDto> _validator = validator;
		private readonly IUserService _userService = userService;

		[HttpPost("signup")]
		public async Task<ActionResult<UserDto>> SignUp([FromBody] SignUpDto dto)
		{
			var validation = await _validator.ValidateAsync(dto);

			if (!validation.IsValid)
			{
				return BadRequest(validation.Errors);
			}

			var createdUser = await _userService.CreateAsync(dto);
			
			return Ok(createdUser);
		}

		//[HttpPost("signin")]
		//public async Task<ActionResult<UserDto>> SignIn([FromBody] SignInDto dto)
		//{

		//}
	}
}
