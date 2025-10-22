using CollabTaskApi.DTOs;
using CollabTaskApi.Mappers;
using CollabTaskApi.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CollabTaskApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _service;

		public UserController(IUserService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
		{
			var users = await _service.GetAll();
			var dtos = users.Select(user => UserMapper.ToDto(user));
			return Ok(dtos);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<UserDto?>> GetById(int id)
		{
			var user = await _service.GetById(id);
			if (user == null) return NotFound();
			return Ok(UserMapper.ToDto(user));
		}

		[HttpPost]
		public async Task<ActionResult<UserDto>> Create(
			[FromBody] UserCreateDto dto,
			[FromServices] IValidator<UserCreateDto> validator)
		{
			var validationResult = await validator.ValidateAsync(dto);
			
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			var createdUser = await _service.Create(UserMapper.ToUser(dto));
			var createdUserDto = UserMapper.ToDto(createdUser);

			return CreatedAtAction(nameof(GetById), new { id = createdUserDto.Id }, createdUserDto);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> Update(
			int id,
			[FromBody] UserUpdateDto dto,
			[FromServices] IValidator<UserUpdateDto> validator)
		{
			var validationResult = await validator.ValidateAsync(dto);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			var user = await _service.GetById(id);
			if (user == null) return NotFound();

			var updatedUser = UserMapper.Update(user, dto);

			updatedUser = await _service.Update(updatedUser);

			if (updatedUser is null)
			{
				return NotFound();
			}

			return NoContent();
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> Delete(int id)
		{
			var success = await _service.Delete(id);

			if (!success) return NotFound();

			return Ok(success);
		}
	}
}
