namespace CollabTaskApi.Shared.Helpers
{
	public interface IPasswordHasher
	{
		string Hash(string original);
		bool Verify(string original, string hashString);
	}
}