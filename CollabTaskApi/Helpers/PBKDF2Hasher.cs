using CollabTaskApi.Helpers.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CollabTaskApi.Helpers
{
	public class PBKDF2Hasher : IPasswordHasher
	{
		private const int SaltSize = 16;
		private const int KeySize = 32;
		private const int Iterations = 10000;
		private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

		public string Hash(string original)
		{
			var salt = RandomNumberGenerator.GetBytes(SaltSize);

			var hash = Rfc2898DeriveBytes.Pbkdf2(
				Encoding.UTF8.GetBytes(original),
				salt,
				Iterations,
				Algorithm,
				KeySize
			);

			return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}.{Iterations}";
		}

		public bool Verify(string original, string hashString)
		{
			var parts = hashString.Split('.');
			if (parts.Length != 3)
				return false;

			var salt = Convert.FromBase64String(parts[0]);
			var hash = Convert.FromBase64String(parts[1]);
			var iterations = int.Parse(parts[2]);

			var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
				Encoding.UTF8.GetBytes(original),
				salt,
				iterations,
				Algorithm,
				KeySize
			);

			return CryptographicOperations.FixedTimeEquals(hash, hashToCompare);
		}
	}
}