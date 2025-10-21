using FluentValidation;

namespace CollabTaskApi.DTOs
{
	public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
	{
		public UserCreateDtoValidator()
		{
			RuleFor(u => u.Name)
				.NotEmpty()
				.MinimumLength(2)
				.MaximumLength(50);

			RuleFor(u => u.Email)
				.NotEmpty()
				.EmailAddress();

			RuleFor(u => u.Password)
				.NotEmpty()
				.MinimumLength(6);
		}
	}
}
