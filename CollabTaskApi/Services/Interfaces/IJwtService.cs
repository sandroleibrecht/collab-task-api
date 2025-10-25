using System.Security.Claims;
using CollabTaskApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace CollabTaskApi.Services.Interfaces
{
	public interface IJwtService
	{
		string GenerateAccessToken(User user, IEnumerable<Claim>? extraClaims = null);
		string GenerateRefreshToken();
		TokenValidationParameters GetValidationParameters();
	}
}
