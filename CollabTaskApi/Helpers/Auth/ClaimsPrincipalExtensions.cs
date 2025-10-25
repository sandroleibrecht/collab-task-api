using System.Security.Claims;

namespace CollabTaskApi.Helpers.Auth
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
