namespace NovelWorld.Utility.Helpers.Abstractions
{
    public interface IPasswordHasher
    {
        string Hash(string password);
  
        (bool Verified, bool NeedsUpgrade) Check(string hash, string password);
    }
}