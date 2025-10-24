using CollabTaskApi.DTOs.Api.Errors;
using FluentValidation.Results;

namespace CollabTaskApi.Mappers.Interfaces
{
	public interface IErrorMapper
	{
		IEnumerable<ValidationErrorDto> Map(List<ValidationFailure> fluentValidationErrors);
	}
}
