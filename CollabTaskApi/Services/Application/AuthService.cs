using CollabTaskApi.DTOs.Auth;
using CollabTaskApi.DTOs.User;
using CollabTaskApi.Helpers.Auth.Interfaces;
using CollabTaskApi.Models;
using CollabTaskApi.Services.Application.Interfaces;

namespace CollabTaskApi.Services.Application
{
	public class AuthService(
		IUserService userService,
		IPasswordHasher hasher,
		IJwtService jwtService) : IAuthService
	{
		private readonly IUserService _userService = userService;
		private readonly IPasswordHasher _hasher = hasher;
		private readonly IJwtService _jwtService = jwtService;

		public async Task<AuthResponseDto?> SignUpAsync(SignUpDto dto)
		{
			var existingUser = await _userService.GetUserByEmailAsync(dto.Email);
			if (existingUser is not null) return null;

			var newUser = new User
			{
				Name = dto.Name.Trim(),
				Email = dto.Email.Trim(),
				Password = _hasher.Hash(dto.Password),
				CreatedAt = DateTime.UtcNow,
			};

			try
			{
				await _userService.CreateAsync(newUser);
				return await BuildAuthResponse(newUser);
			}
			catch (Exception ex)
			{
				await _userService.DeleteAsync(newUser.Id);
				throw new InvalidOperationException("Signup error. User not created.", ex);
			}
		}

		public async Task<AuthResponseDto?> SignInAsync(SignInDto dto)
		{
			var user = await _userService.GetUserByEmailAsync(dto.Email);
			if (user is null) return null;

			if (!_hasher.Verify(dto.Password, user.Password)) return null;

			return await BuildAuthResponse(user);
		}

		public async Task<AuthResponseDto> BuildAuthResponse(User user)
		{
			var access = _jwtService.GenerateAccessToken(user);
			var refresh = await _jwtService.GenerateAndSaveRefreshTokenAsync(user);

			return new AuthResponseDto
			{
				AccessToken = access,
				RefreshToken = refresh.Token,
				User = new UserDto
				{
					Id = user.Id,
					Name = user.Name,
					Email = user.Email
				}
			};
		}
	}
}
