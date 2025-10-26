using CollabTaskApi.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.Results;
using CollabTaskApi.Services.Application.Interfaces;

namespace CollabTaskApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(
		IValidator<SignUpDto> signUpValidator,
		IValidator<SignInDto> signInValidator,
		IAuthService authService,
		IJwtService jwtService) : ControllerBase
	{
		private readonly IValidator<SignUpDto> _signUpValidator = signUpValidator;
		private readonly IValidator<SignInDto> _signInValidator = signInValidator;
		private readonly IAuthService _authService = authService;
		private readonly IJwtService _jwtService = jwtService;

		[HttpPost("signup")]
		[ProducesResponseType<List<ValidationFailure>>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType<AuthResponseDto>(StatusCodes.Status200OK)]
		public async Task<ActionResult<AuthResponseDto>> SignUp([FromBody] SignUpDto dto)
		{
			var res = await _signUpValidator.ValidateAsync(dto);
			if (!res.IsValid) return BadRequest(res.Errors);

			var auth = await _authService.SignUpAsync(dto);
			if (auth is null) return BadRequest();

			return Ok(auth);
		}

		[HttpPost("signin")]
		[ProducesResponseType<List<ValidationFailure>>(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<AuthResponseDto>(StatusCodes.Status200OK)]
		public async Task<ActionResult<AuthResponseDto>> SignIn([FromBody] SignInDto dto)
		{
			var res = await _signInValidator.ValidateAsync(dto);
			if (!res.IsValid) return BadRequest(res.Errors);

			var auth = await _authService.SignInAsync(dto);
			if (auth is null) return Unauthorized();

			return Ok(auth);
		}

		[HttpPost("refresh")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType<AuthResponseDto>(StatusCodes.Status200OK)]
		public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenRequestDto dto)
		{
			var token = await _jwtService.ValidateRefreshToken(dto);
			if (token is null) return Unauthorized();

			var auth = await _authService.BuildAuthResponse(token.User);
			if (auth is null) return Unauthorized();

			return Ok(auth);
		}
	}
}
