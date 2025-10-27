using CollabTaskApi.Domain.DTOs.Auth;
using FluentValidation;
using System.Text.RegularExpressions;

namespace CollabTaskApi.Domain.Validation
{
	public class SignInDtoValidator : AbstractValidator<SignInDto>
	{
		public SignInDtoValidator()
		{
			RuleFor(x => x.Email)
				.Cascade(CascadeMode.Stop)
				.NotEmpty()
				.Must(email =>
					!string.IsNullOrWhiteSpace(email)
					&& Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
				.WithMessage("Invalid email address");
		}
	}
}
