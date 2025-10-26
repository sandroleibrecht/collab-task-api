using CollabTaskApi.Domain.DTOs.Auth;
using CollabTaskApi.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CollabTaskApi.Application.Interfaces
{
	public interface IJwtService
	{
		string GenerateAccessToken(User user, IEnumerable<Claim>? extraClaims = null);
		Task<UserRefreshToken> GenerateAndSaveRefreshTokenAsync(User user);
		Task<UserRefreshToken?> ValidateRefreshToken(RefreshTokenRequestDto dto);
		TokenValidationParameters GetValidationParameters();
	}
}
