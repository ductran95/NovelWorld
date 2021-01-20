namespace NovelWorld.Utility.Helpers.Abstractions
{
    public interface ICryptoHelper
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}