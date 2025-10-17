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
		private readonly UserService _service;

		public UserController(UserService service)
		{
			_service = service;
		}

		[HttpPost]
		public ActionResult CreateUser(UserCreateDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = _service.Create(dto);

			return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, user);
		}

		[HttpGet]
		public ActionResult<IEnumerable<User>> GetAllUsers()
		{
			var users = _service.GetAll();
			return Ok(users);
		}
	}
}
