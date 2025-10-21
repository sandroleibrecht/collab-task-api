using CollabTaskApi.DTOs;
using CollabTaskApi.Models;
using CollabTaskApi.Services;
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

			var dtos = users.Select(u => new UserDto
			{
				Id = u.Id,
				Name = u.Name,
				Email = u.Email,
			});

			return Ok(dtos);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<UserDto?>> GetById(int id)
		{
			var user = await _service.GetById(id);
			if (user == null) return NotFound();
			return Ok(new UserDto
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email
			});
		}

		[HttpPost]
		public async Task<ActionResult<UserDto>> Create([FromBody] UserCreateDto dto)
		{
			var userToCreate = new User
			{
				Name = dto.Name,
				Email = dto.Email,
			};

			var createdUser = await _service.Create(userToCreate);
			return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
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
