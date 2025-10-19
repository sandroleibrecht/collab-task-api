using CollabTaskApi.Models;
using CollabTaskApi.DTOs;
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
		public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
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

		[HttpPost]
		public async Task<ActionResult<UserDto>> CreateUser(UserCreateDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var createdUser = await _service.Create(new User { Name = dto.Name, Email = dto.Email });
			return CreatedAtAction(nameof(GetAllUsers), new { id = createdUser.Id }, createdUser);
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteUser(int id)
		{
			var success = await _service.Delete(id);

			if (!success) return NotFound();

			return Ok(success);
		}
	}
}
