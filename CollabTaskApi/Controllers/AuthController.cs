using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.DTOs.User;
using CollabTaskApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace CollabTaskApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(IValidator<SignUpDto> signUpValidator, IValidator<SignInDto> signInValidator, IUserService userService) : ControllerBase
	{
		private readonly IValidator<SignUpDto> _signUpValidator = signUpValidator;
		private readonly IValidator<SignInDto> _signInValidator = signInValidator;
		private readonly IUserService _userService = userService;

		[HttpPost("signup")]
		[Consumes("application/json")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<UserDto>> SignUp([FromBody] SignUpDto dto)
		{
			var validation = await _signUpValidator.ValidateAsync(dto);

			if (!validation.IsValid)
			{
				return BadRequest(validation.Errors);
			}

			var createdUser = await _userService.CreateAsync(dto);
			
			return Ok(createdUser);
		}

		[HttpPost("signin")]
		[Consumes("application/json")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<AuthResponseDto>> SignIn([FromBody] SignInDto dto)
		{
			var validation = await _signInValidator.ValidateAsync(dto);

			if (!validation.IsValid)
			{
				return BadRequest(validation.Errors);
			}

			var authResponse = await _userService.SignInAsync(dto);
			
			if (authResponse is null)
			{
				return Unauthorized();
			}

			return Ok(authResponse);
		}
	}
}
