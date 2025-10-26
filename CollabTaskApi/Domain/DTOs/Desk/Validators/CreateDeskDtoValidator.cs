using CollabTaskApi.Domain.DTOs.Desk;
using FluentValidation;

namespace CollabTaskApi.Domain.DTOs.Desk.Validators
{
	public class CreateDeskDtoValidator : AbstractValidator<CreateDeskDto>
	{
		public CreateDeskDtoValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.MaximumLength(30);

			RuleFor(x => x.Color)
				.Matches(@"^#(?:[0-9a-fA-F]{3}){1,2}$")
				.When(x => !string.IsNullOrWhiteSpace(x.Color))
				.WithMessage("Color must be a valid hex code.");
		}
	}
}
