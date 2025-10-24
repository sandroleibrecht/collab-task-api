namespace CollabTaskApi.DTOs.Api.Errors
{
	public class ValidationErrorDto : ApiErrorDto
	{
		public string Field { get; set; } = string.Empty;
	}
}
