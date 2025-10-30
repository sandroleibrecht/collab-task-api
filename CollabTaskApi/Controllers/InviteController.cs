using CollabTaskApi.Application.Interfaces;
using CollabTaskApi.Domain.DTOs.Board;
using CollabTaskApi.Domain.DTOs.Invites;
using CollabTaskApi.Shared.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollabTaskApi.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class InviteController(IInviteService inviteService, IValidator<SendInvitationDto> sendInvitationDtoValidator) : ControllerBase
	{
		private readonly IInviteService _inviteService = inviteService;
		private readonly IValidator<SendInvitationDto> _sendInvitationDtoValidator = sendInvitationDtoValidator;

		[HttpPost("send")]
		public async Task<IActionResult> SendInvitation([FromBody] SendInvitationDto dto)
		{
			var userId = User.GetUserId();
			if (userId is null) return Unauthorized();

			var validation = await _sendInvitationDtoValidator.ValidateAsync(dto);
			if (!validation.IsValid) return BadRequest(validation.Errors);

			await _inviteService.SendInvitationAsync((int)userId, dto.ReceiverEmail, dto.DeskId);

			return NoContent();
		}

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
