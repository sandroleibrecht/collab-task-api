using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CollabTaskApi.Models;
using CollabTaskApi.Options;
using CollabTaskApi.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace CollabTaskApi.Services
{
	public class JwtService : IJwtService
	{
		private readonly JwtOptions _options;
		private readonly SymmetricSecurityKey _signingKey;

		public JwtService(IOptions<JwtOptions> options)
		{
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

		public string GenerateRefreshToken()
		{
			return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
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

