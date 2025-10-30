using CollabTaskApi.Domain.DTOs.Invites;
using FluentValidation;
using System.Text.RegularExpressions;

namespace CollabTaskApi.Domain.Validation
{
	public class SendInvitationDtoValidatior : AbstractValidator<SendInvitationDto>
	{
		public SendInvitationDtoValidatior()
		{
			RuleFor(x => x.ReceiverEmail)
				.Cascade(CascadeMode.Stop)
				.NotEmpty()
				.Must(email =>
					!string.IsNullOrWhiteSpace(email)
					&& Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
				.WithMessage("Invalid email address");

			RuleFor(x => x.DeskId)
				.NotEmpty();
		}
	}
}
