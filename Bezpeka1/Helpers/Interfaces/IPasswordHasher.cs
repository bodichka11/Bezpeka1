namespace Bezpeka1.Helpers.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash1(string password);

        string Hash(string password);

        bool Verify(string password, string hash);

        bool Verify1(string password, string hash);
    }
}
