using CollabTask.Api.Shared.Helpers;

namespace CollabTask.Api.Tests
{
	public abstract class PasswordHasherTests
	{
		protected abstract IPasswordHasher CreateHasher();

		[Fact]
		public void HashPassword_ShouldNotReturnPlaintext()
		{
			var hasher = CreateHasher();
			var password = "SafeTestPassword123!";
			var hash = hasher.Hash(password);

			Assert.NotEqual(password, hash);
		}

		[Fact]
		public void VerifyPasswordWithHash_ShouldReturnTrue()
		{
			var hasher = CreateHasher();
			var password = "SafeTestPassword123!";
			var hash = hasher.Hash(password);

			var isVerified = hasher.Verify(password, hash);
			
			Assert.True(isVerified);
		}

		[Fact]
		public void VerifyPassword_WithWrongPassword_ShouldReturnFalse()
		{
			var hasher = CreateHasher();
			var password = "SafeTestPassword123!";
			var hash = hasher.Hash(password);

			var wrongPassword = "WrongPassword123!";
			var isVerified = hasher.Verify(wrongPassword, hash);
			
			Assert.False(isVerified);
		}
	}

	public class Argon2PasswordHasherTests : PasswordHasherTests
	{
		protected override IPasswordHasher CreateHasher() => new Argon2PasswordHasher();
	}
}
