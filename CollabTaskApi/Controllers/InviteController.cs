using CollabTaskApi.Application.Interfaces;
using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollabTaskApi.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class InviteController(IInviteService inviteService) : ControllerBase
	{
		private readonly IInviteService _inviteService = inviteService;

		[HttpPost("accept/{inviteId:int}")]
		public async Task<ActionResult<BoardDeskDto>> AcceptInvitation(int inviteId)
		{
			var userId = User.GetUserId();
			if (userId is null) return Unauthorized();

			var dto = await _inviteService.AcceptInvitationAsync(inviteId);
			
			return Ok(dto);
		}

		[HttpPost("decline/{inviteId:int}")]
		public async Task<IActionResult> DeclineInvitation(int inviteId)
		{
			var userId = User.GetUserId();
			if (userId is null) return Unauthorized();

			await _inviteService.DeclineInvitationAsync(inviteId);

			return NoContent();
		}
	}
}
