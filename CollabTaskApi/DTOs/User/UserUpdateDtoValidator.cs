using FluentValidation;

namespace CollabTaskApi.DTOs.User
{
	public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
	{
		public UserUpdateDtoValidator()
		{
			When(u => u.Name != null, () =>
			{
				RuleFor(u => u.Name)
					.MinimumLength(2)
					.MaximumLength(50);
			});

			When(u => u.Email != null, () =>
			{
				RuleFor(u => u.Email)
					.EmailAddress();
			});

			When(u => u.Password != null, () =>
			{
				RuleFor(u => u.Password)
					.MinimumLength(6);
			});
		}
	}
}
