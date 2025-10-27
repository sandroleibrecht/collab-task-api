using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace CollabTaskApi.Shared.Helpers
{
	public class Argon2PasswordHasher : IPasswordHasher
	{
		public string Hash(string password)
		{
			var salt = RandomNumberGenerator.GetBytes(16);

			var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
			{
				Salt = salt,
				DegreeOfParallelism = 8,
				MemorySize = 64 * 1024,
				Iterations = 4
			};

			var hash = argon2.GetBytes(32);
			var combined = new byte[salt.Length + hash.Length];
			Buffer.BlockCopy(salt, 0, combined, 0, salt.Length);
			Buffer.BlockCopy(hash, 0, combined, salt.Length, hash.Length);

			return Convert.ToBase64String(combined);
		}

		public bool Verify(string password, string hashed)
		{
			var decoded = Convert.FromBase64String(hashed);
			var salt = new byte[16];
			Buffer.BlockCopy(decoded, 0, salt, 0, salt.Length);
			var storedHash = new byte[decoded.Length - salt.Length];
			Buffer.BlockCopy(decoded, salt.Length, storedHash, 0, storedHash.Length);

			var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
			{
				Salt = salt,
				DegreeOfParallelism = 8,
				MemorySize = 64 * 1024,
				Iterations = 4
			};

			var computed = argon2.GetBytes(32);
			return CryptographicOperations.FixedTimeEquals(storedHash, computed);
		}
	}
}