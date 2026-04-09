using CollabTask.Api.Domain.DTOs.Auth;
using CollabTask.Api.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CollabTask.Api.Application.Interfaces
{
	public interface IJwtService
	{
		string GenerateAccessToken(User user, IEnumerable<Claim>? extraClaims = null);
		Task<UserRefreshToken> GenerateAndSaveRefreshTokenAsync(User user);
		Task<UserRefreshToken?> ValidateRefreshToken(RefreshTokenRequestDto dto);
		Task RemoveRefreshTokenAsync(int userId);
		TokenValidationParameters GetValidationParameters();
	}
}
