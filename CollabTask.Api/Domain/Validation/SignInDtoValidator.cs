using FluentValidation;
using System.Text.RegularExpressions;
using CollabTask.Shared.DTOs.Auth;

namespace CollabTask.Api.Domain.Validation
{
	public class SignInDtoValidator : AbstractValidator<SignInDto>
	{
		public SignInDtoValidator()
		{
			RuleFor(x => x.Email)
				.Cascade(CascadeMode.Stop)
				.NotEmpty()
				.Must(email => !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
				.WithMessage("Invalid email address");
		}
	}
}
