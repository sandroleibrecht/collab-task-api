using CollabTaskApi.Application.Interfaces;
using CollabTaskApi.Domain.DTOs.Auth;
using CollabTaskApi.Domain.Models;
using CollabTaskApi.Infrastructure.Configuration;
using CollabTaskApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CollabTaskApi.Application.Services
{
	public class JwtService : IJwtService
	{
		private readonly AppDbContext _context;
		private readonly JwtOptions _options;
		private readonly SymmetricSecurityKey _signingKey;

		public JwtService(AppDbContext context, IOptions<JwtOptions> options)
		{
			_context = context;
			_options = options.Value;
			_signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
		}

		public string GenerateAccessToken(User user, IEnumerable<Claim>? extraClaims = null)
		{
			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new(JwtRegisteredClaimNames.Email, user.Email),
				new("name", user.Name)
			};

			if (extraClaims != null) claims.AddRange(extraClaims);

			var creds = new SigningCredentials(_signingKey,SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _options.Issuer,
				audience: _options.Audience,
				claims: claims,
				notBefore: DateTime.UtcNow,
				expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<UserRefreshToken> GenerateAndSaveRefreshTokenAsync(User user)
		{
			var token = new UserRefreshToken
			{
				Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
				ExpiresAt = DateTime.UtcNow.AddDays(_options.RefreshTokenDays),
				CreatedAt = DateTime.UtcNow,
				UserId = user.Id
			};

			await _context.UserRefreshToken.AddAsync(token);
			await _context.SaveChangesAsync();

			return token;
		}

		public async Task<UserRefreshToken?> ValidateRefreshToken(RefreshTokenRequestDto dto)
		{
			var refreshToken = await _context.UserRefreshToken
				.Include(t => t.User)
				.FirstOrDefaultAsync(t => t.Token == dto.RefreshToken);
			
			if (refreshToken == null || refreshToken.ExpiresAt < DateTime.UtcNow)
			{
				return null;
			}

			return refreshToken;
		}

		public TokenValidationParameters GetValidationParameters() => new()
		{
			ValidateIssuer = true,
			ValidIssuer = _options.Issuer,
			ValidateAudience = true,
			ValidAudience = _options.Audience,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = _signingKey,
			ValidateLifetime = true,
			ClockSkew = TimeSpan.FromSeconds(30)
		};
	}
}

