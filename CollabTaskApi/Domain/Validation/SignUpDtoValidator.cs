using CollabTaskApi.Domain.DTOs.Auth;
using FluentValidation;
using System.Text.RegularExpressions;

namespace CollabTaskApi.Domain.Validation
{
	public class SignUpDtoValidator : AbstractValidator<SignUpDto>
	{
		public SignUpDtoValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.Must(name => (name is not null && name.Trim().Length > 2))
				.WithMessage("Name must have a minimum length of 2 characters");

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
