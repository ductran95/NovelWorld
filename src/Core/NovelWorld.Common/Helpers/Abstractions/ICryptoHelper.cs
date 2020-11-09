namespace NovelWorld.Common.Helpers.Abstractions
{
    public interface ICryptoHelper
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}