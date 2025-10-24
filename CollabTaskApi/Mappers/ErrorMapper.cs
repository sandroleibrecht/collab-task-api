using CollabTaskApi.DTOs.Api.Errors;
using CollabTaskApi.Mappers.Interfaces;
using FluentValidation.Results;

namespace CollabTaskApi.Mappers
{
	public class ErrorMapper : IErrorMapper
	{
		public IEnumerable<ValidationErrorDto> Map(List<ValidationFailure> fluentValidationErrors)
		{
			return [.. fluentValidationErrors.Select(err => new ValidationErrorDto
			{
				Type = "ValidationError",
				Message = err.ErrorMessage,
				Field = err.PropertyName
			})];
		}
	}
}
