using CollabTaskApi.Models;
using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.Helpers.Interfaces;
using CollabTaskApi.Mappers.Interfaces;
using CollabTaskApi.Services.Interfaces;

namespace CollabTaskApi.Services
{
	public class AuthService(
		IUserService userService,
		IUserMapper userMapper,
		IPasswordHasher hasher) : IAuthService
	{
		private readonly IUserService _userService = userService;
		private readonly IUserMapper _userMapper = userMapper;
		private readonly IPasswordHasher _hasher = hasher;

		public async Task<AuthResponseDto?> SignUpAsync(SignUpDto dto)
		{
			var existingUser = await _userService.GetUserByEmailAsync(dto.Email);

			if (existingUser is not null)
			{
				return null;
			}

			var passwordHash = _hasher.Hash(dto.Password);

			var newUser = new User
			{
				Name = dto.Name.Trim(),
				Email = dto.Email.Trim(),
				Password = passwordHash,
				CreatedAt = DateTime.UtcNow,
			};

			bool success = await _userService.CreateAsync(newUser);

			if (!success)
			{
				return null;
			}


			// --- jwt system tbd


			return new AuthResponseDto
			{
				AccessToken = "TestAccessTokekn",
				RefreshToken = "TestRefreshToken",
				User = _userMapper.Map(newUser)
			};
		}

		public async Task<AuthResponseDto?> SignInAsync(SignInDto dto)
		{
			var user = await _userService.GetUserByEmailAsync(dto.Email);

			if (user is null) return null;

			bool passwordMatch = _hasher.Verify(dto.Password, user.Password);
			
			if (!passwordMatch) return null;


			// --- jwt system tbd


			return new AuthResponseDto
			{
				AccessToken = "TestAccessToken",
				RefreshToken = "TestRefreshToken",
				User = _userMapper.Map(user)
			};
		}
	}
}
