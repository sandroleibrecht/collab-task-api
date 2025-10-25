namespace CollabTaskApi.Helpers.Interfaces
{
	public interface IPasswordHasher
	{
		string Hash(string original);
		bool Verify(string original, string hashString);
	}
}