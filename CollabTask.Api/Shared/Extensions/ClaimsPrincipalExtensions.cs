using System.Security.Claims;

namespace CollabTask.Api.Shared.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
		public static int? GetUserId(this ClaimsPrincipal user)
		{
			var val = user.FindFirstValue(ClaimTypes.NameIdentifier);
			return int.TryParse(val, out int id) ? id : null;
		}
	}
}
