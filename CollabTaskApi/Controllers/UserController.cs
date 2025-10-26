using CollabTaskApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollabTaskApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController(IUserService userService) : ControllerBase
	{
		private readonly IUserService _userService = userService;

		// set image, update user, ...
	}
}
