using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CollabTaskApi.Services.Application.Interfaces
{
	public interface IJwtService
	{
		string GenerateAccessToken(User user, IEnumerable<Claim>? extraClaims = null);
		Task<UserRefreshToken> GenerateAndSaveRefreshTokenAsync(User user);
		Task<UserRefreshToken?> ValidateRefreshToken(RefreshTokenRequestDto dto);
		TokenValidationParameters GetValidationParameters();
	}
}
