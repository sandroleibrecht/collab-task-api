using CollabTaskApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CollabTaskApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WorkspaceController : ControllerBase
	{
		private readonly IWorkspaceService _service;

		public WorkspaceController(IWorkspaceService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var workspaces = await _service.GetAll();
			return Ok(workspaces);
		}

		// Service already prepared - endpoints TBD
		// ...
		// ...
	}
}
