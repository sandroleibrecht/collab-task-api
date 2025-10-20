using CollabTaskApi.DTOs;
using CollabTaskApi.Models;
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
		public async Task<ActionResult<IEnumerable<WorkspaceDto>>> GetAll()
		{
			var workspaces = await _service.GetAll();

			var dtos = workspaces.Select(w => new WorkspaceDto
			{
				Id = w.Id,
				Name = w.Name,
				Description = w.Description
			}); 

			return Ok(dtos);
		}

		[HttpPost]
		public async Task<ActionResult<WorkspaceDto>> Create(WorkspaceCreateDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var workspace = await _service.Create(new Workspace { Name = dto.Name, Description = dto.Description });

			var workspaceDto = new WorkspaceDto
			{
				Id = workspace.Id,
				Name = workspace.Name,
				Description = workspace.Description
			};

			return Ok(workspaceDto);
		}

		[HttpDelete]
		public async Task<ActionResult<bool>> Delete(int id)
		{
			bool success = await _service.Delete(id);
			return success ? Ok(success) : BadRequest();
		}
	}
}