using CollabTaskApi.Domain.DTOs.Auth;
using FluentValidation;
using System.Text.RegularExpressions;

namespace CollabTaskApi.Domain.DTOs.Auth.Validators
{
	public class SignUpDtoValidator : AbstractValidator<SignUpDto>
	{
		public SignUpDtoValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.MinimumLength(2);

			RuleFor(x => x.Email)
				.Cascade(CascadeMode.Stop)
				.NotEmpty()
				.Must(email =>
					!string.IsNullOrWhiteSpace(email)
					&& Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
				.WithMessage("Invalid email address");

			RuleFor(x => x.Password)
				.NotEmpty()
				.MinimumLength(6);
		}
	}
}
