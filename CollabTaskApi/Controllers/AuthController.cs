using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.DTOs.User;
using CollabTaskApi.DTOs.Api.Errors;
using CollabTaskApi.Services.Interfaces;
using CollabTaskApi.Mappers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace CollabTaskApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(
		IErrorMapper errorMapper,
		IValidator<SignUpDto> signUpValidator,
		IValidator<SignInDto> signInValidator,
		IAuthService authService,
		IUserService userService) : ControllerBase
	{
		private readonly IErrorMapper _errorMapper = errorMapper;
		private readonly IValidator<SignUpDto> _signUpValidator = signUpValidator;
		private readonly IValidator<SignInDto> _signInValidator = signInValidator;
		private readonly IAuthService _authService = authService;
		private readonly IUserService _userService = userService;

		[HttpPost("signup")]
		[Consumes("application/json")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(IEnumerable<ValidationErrorDto>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
		public async Task<ActionResult<UserDto>> SignUp([FromBody] SignUpDto dto)
		{
			var validation = await _signUpValidator.ValidateAsync(dto);

			if (!validation.IsValid)
			{
				return BadRequest(_errorMapper.Map(validation.Errors));
			}

			var authResponse = await _authService.SignUpAsync(dto);

			if (authResponse is null)
			{
				return BadRequest(/*new ApiErrorDto { Type = "RegistrationError", Message = ""}*/);
			}

			return Ok(authResponse);
		}

		[HttpPost("signin")]
		[Consumes("application/json")]
		[Produces("application/json")]
		[ProducesResponseType(typeof(IEnumerable<ValidationErrorDto>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
		public async Task<ActionResult<AuthResponseDto>> SignIn([FromBody] SignInDto dto)
		{
			var validation = await _signInValidator.ValidateAsync(dto);

			if (!validation.IsValid)
			{
				return BadRequest(_errorMapper.Map(validation.Errors));
			}

			var authResponse = await _authService.SignInAsync(dto);
			
			if (authResponse is null)
			{
				return Unauthorized(/*new ApiErrorDto { Type = "AuthenticationError" }*/);
			}

			return Ok(authResponse);
		}
	}
}
