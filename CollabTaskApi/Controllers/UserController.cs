using AutoMapper;
using CollabTaskApi.DTOs;
using CollabTaskApi.Models;
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
		private readonly IMapper _mapper;

		public UserController(IUserService service, IMapper mapper)
		{
			_service = service;
			_mapper = mapper;
		}
		
		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
		{
			var users = await _service.GetAll();
			var dtos = users.Select(user => _mapper.Map<UserDto>(user));
			return Ok(dtos);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<UserDto?>> GetById(int id)
		{
			var user = await _service.GetById(id);
			if (user == null) return NotFound();
			var userDto = _mapper.Map<UserDto>(user);
			return Ok(userDto);
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

			var userToCreate = _mapper.Map<User>(dto);
			var createdUserDto = _mapper.Map<UserDto>(await _service.Create(userToCreate));

			return CreatedAtAction(nameof(GetById), new { id = createdUserDto.Id }, createdUserDto);
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<UserDto?>> Update(int id, [FromBody] UserUpdateDto dto)
		{
			var userModel = await _service.GetById(id);

			if (userModel == null) return NotFound();

			userModel.Name = dto.Name ?? userModel.Name;
			userModel.Email = dto.Email ?? userModel.Email;
			
			var updatedUser = _service.Update(userModel);

			if (updatedUser == null) return NotFound();
			return Ok(updatedUser);
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
