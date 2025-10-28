using CollabTaskApi.Domain.DTOs.User;
using FluentValidation;
using System.Text.RegularExpressions;

namespace CollabTaskApi.Domain.Validation
{
	public class UserUpdateDtoValidatior : AbstractValidator<UserUpdateDto>
	{
		private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png" };
		private const long MaxImageFileSize = 2 * 1024 * 1024; // 2 MB

		public UserUpdateDtoValidatior()
		{
			RuleFor(x => x).Custom((dto, context) =>
			{
				if (dto.Name is null && dto.Email is null && dto.Password is null && dto.Image is null)
				{
					context.AddFailure("Mindestens ein Feld (Name, Email, Passwort oder Bild) muss angegeben werden.");
				}
			});

			When(x => x.Name is not null, () =>
			{
				RuleFor(x => x.Name)
					.NotEmpty()
					.Must(name => name!.Trim().Length > 2).WithMessage("Name must have a minimum length of 2 characters");
			});

			When(x => x.Email is not null, () =>
			{
				RuleFor(x => x.Email)
					.Cascade(CascadeMode.Stop)
					.NotEmpty()
					.Must(email =>
						!string.IsNullOrWhiteSpace(email)
						&& Regex.IsMatch(email.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
					.WithMessage("Invalid email address");
			});

			When(x => x.Password is not null, () =>
			{
				RuleFor(x => x.Password)
					.NotEmpty()
					.MinimumLength(6);
			});

			When(x => x.Image is not null, () =>
			{
				RuleFor(x => x.Image).MinimumLength(1);
				//RuleFor(x => x.Image)
				//	.Must(file => file!.Length > 0)
				//		.WithMessage("File is empty")
				//	.Must(file => AllowedImageExtensions.Contains(Path.GetExtension(file!.FileName).ToLower()))
				//		.WithMessage($"Image format not allowed. Use {string.Join(", ", AllowedImageExtensions)}")
				//	.Must(file => file!.Length <= MaxImageFileSize)
				//		.WithMessage($"Image size not allowed. Max. {MaxImageFileSize / 1024 / 1024} MB.")
				//	.Must(file => file!.ContentType.StartsWith("image/"))
				//		.WithMessage("File has to be of type Image");
			});



		}
	}
}
